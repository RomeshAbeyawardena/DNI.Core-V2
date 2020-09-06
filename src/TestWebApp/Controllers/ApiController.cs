using DNI.Core.Contracts.Providers;
using DNI.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

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
