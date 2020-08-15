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
    [Route("api/{version}/{controller}/{action}")]
    public abstract class ApiController : ExtendedControllerBase
    {
        protected ApiController(
            IMediatorProvider mediator, 
            IMapperProvider mapperProvider) 
            : base(mediator, mapperProvider)
        {
        }
    }
}
