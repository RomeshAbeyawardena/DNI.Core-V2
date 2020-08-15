using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Abstractions;
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

        public UserController(IMediatorProvider mediator, IMapperProvider mapperProvider, IRepository<User> userRepository) 
            : base(mediator, mapperProvider)
        {
            this.userRepository = userRepository;
        }

        [Version("1.0", "1.9")]
        public ActionResult Hello()
        {
            return Ok("Hello");
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
