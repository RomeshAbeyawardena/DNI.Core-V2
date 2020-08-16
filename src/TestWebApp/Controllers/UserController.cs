using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Abstractions;
using DNI.Core.Services.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp.Controllers
{
    
    public class UserController : ApiController
    {
        private readonly IRepository<User> userRepository;
        private readonly IModelEncryptionProvider encryptionService;
        public UserController(
            IMediatorProvider mediator, 
            IMapperProvider mapperProvider, 
            IRepository<User> userRepository,
            IModelEncryptionProvider encryptionService) 
            : base(mediator, mapperProvider)
        {
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
        }

        [Version("1.0", "1.9")]
        public ActionResult Hello()
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

            var decryptedUser = encryptionService.Decrypt(savedUser);

            decryptedUser.MiddleName = "Romesh2";

            var encrypted = encryptionService.Encrypt(decryptedUser);

            userRepository.SaveChanges(encrypted);

            savedUser = userRepository.Query.FirstOrDefault(user => user.FirstName == encryptedUser.FirstName);

            decryptedUser = encryptionService.Decrypt(savedUser);

            return Ok(decryptedUser);
        }

        [Version("2.0", "2.9"), ActionName("Hello")]
        public ActionResult HelloV2()
        {
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
