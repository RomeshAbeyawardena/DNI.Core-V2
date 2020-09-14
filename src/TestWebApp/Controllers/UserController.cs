using DNI.Core.Contracts;
using DNI.Core.Contracts.Collectors;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Attributes;
using DNI.Core.Shared.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestWebApp.Controllers
{

    public class UserController : ApiController
    {
        private readonly ILogger<UserController> loggger;
        private readonly ITypeCollector serviceCollector;
        private readonly IRepository<User> userRepository;
        private readonly IModelEncryptionProvider encryptionService;
        private readonly IJsonTokenService jsonTokenService;
        private readonly IRepositoryOptions repositoryOptions;
        private readonly ICacheManager cacheManager;

        public UserController(
            ILogger<UserController> loggger,
            ITypeCollector serviceCollector,
            IMediatorProvider mediator, 
            IMapperProvider mapperProvider, 
            IRepository<User> userRepository,
            IModelEncryptionProvider encryptionService,
            IJsonTokenService jsonTokenService,
            IRepositoryOptions repositoryOptions,
            ICacheManager cacheManager) 
            : base(mediator, mapperProvider)
        {
            this.loggger = loggger;
            this.serviceCollector = serviceCollector;
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
            this.jsonTokenService = jsonTokenService;
            this.repositoryOptions = repositoryOptions;

            this.cacheManager = cacheManager;
        }

        [Version("1.0", "1.9")]
        public async Task<ActionResult> Hello()
        {
            var encryptedUser = encryptionService
                .Encrypt(new User { 
                    Id = 1,
                    EmailAddress = "romesh.abeyawardena@dotnetinsights.net", 
                    FirstName = "Anthony",
                    MiddleName = "Romesh",
                    LastName = "Abeyawardena",
                    Password = "P@s$w0rd0_2$"
                });

            var savedUser = userRepository.Query.FirstOrDefault(user => user.FirstName == encryptedUser.FirstName);

            if(savedUser == null)
            {
                encryptedUser.Id = 0;
                userRepository.SaveChanges(encryptedUser);

                savedUser = userRepository.Query.FirstOrDefault(user => user.FirstName == encryptedUser.FirstName);
            }

            var decryptedUser = encryptionService.Decrypt(savedUser);

            decryptedUser.MiddleName = "Romesh2";

            var encrypted = encryptionService.Encrypt(decryptedUser);

            userRepository.SaveChanges(encrypted);

            savedUser = userRepository.Query.FirstOrDefault(user => user.FirstName == encryptedUser.FirstName);

            decryptedUser = encryptionService.Decrypt(savedUser);

            var distributedCache = cacheManager.GetAsyncCacheService(CacheType.DistributedCache);

            await distributedCache.SetAsync("decryptedUser", decryptedUser, CancellationToken.None);

            var decrypted = await distributedCache.GetAsync<User>("decryptedUser", CancellationToken.None);

            return Ok(decryptedUser);
        }

        [Version("2.0", "2.9"), ActionName("Hello")]
        public ActionResult HelloV2()
        {
            loggger.LogInformation("test");
            return Ok("Hello v2");
        }

        [Version("3.0"), ActionName("Hello")]
        public ActionResult HelloV3()
        {
            return Ok("Hello v3");
        }


        public ActionResult GoodBye()
        {
            return Ok("Bye!");
        }
    }
}
