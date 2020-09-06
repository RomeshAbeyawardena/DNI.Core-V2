using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts;
using System;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ExtendedControllerBase : ControllerBase
    {
        protected ExtendedControllerBase(IMediatorProvider mediator, IMapperProvider mapperProvider)
        {
            Mapper = mapperProvider;
            Mediator = mediator;
        }

        protected IMediatorProvider Mediator;
        protected IMapperProvider Mapper { get; }

        protected IActionResult ValidateResponse<T> (
            IResponse<T> response, 
            Func<T, IActionResult> runOnSuccessful = null,
            Func<T, Exception, IActionResult> runOnFailure = null)
        {
            if(runOnSuccessful == null)
            {
                runOnSuccessful = result => Ok(result);
            }

            if(runOnFailure == null)
            {
                runOnFailure = (result, exception) => BadRequest(exception);
            }

            if (response.IsSuccessful)
            {
                return runOnSuccessful(response.Result);
            }

            return runOnFailure(response.Result, response.Exception);
        }
    }
}
