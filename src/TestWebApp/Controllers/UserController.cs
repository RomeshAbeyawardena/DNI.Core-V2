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
        public UserController(IMediatorProvider mediator, IMapperProvider mapperProvider) 
            : base(mediator, mapperProvider)
        {

        }

        [Version("1.0", "1.4")]
        public ActionResult Hello()
        {
            return Ok("Hello");
        }

        [Version("2.0"), ActionName("Hello")]
        public ActionResult HelloV2()
        {
            return Ok("Hello v2");
        }

        public ActionResult GoodBye()
        {
            return Ok("Bye!");
        }
    }
}
