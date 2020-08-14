using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Core.Services.Abstractions
{
    public abstract class ExtendedControllerBase : ControllerBase
    {
        protected ExtendedControllerBase(IMapperProvider mapperProvider)
        {
            Mapper = mapperProvider;
        }

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
