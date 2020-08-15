using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = DNI.Core.Domains.Version;

namespace TestWebApp
{

    public sealed class VersionAttribute : ActionFilterAttribute
    {
        public VersionAttribute(string minimum, string maximum = null)
        {

            if (!Version.TryParse(minimum, out var minimumVersion))
            {
                throw new ArgumentException($"{minimum} not in valid format ([Major].[Minor])", nameof(minimum));
            }

            if (maximum != null & !Version.TryParse(minimum, out var maximumVersion))
            {
                throw new ArgumentException($"{maximum} not in valid format ([Major].[Minor])", nameof(maximum));
            }

            Minimum = minimumVersion;
            Maximum = maximumVersion;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (!context.RouteData.Values.TryGetValue("version", out var versionString))
                {
                    throw new InvalidOperationException();
                }

                if (!Version.TryParse(versionString.ToString(), out var version))
                {
                    throw new InvalidOperationException();
                }

                if (Version.IsInRange(version, Minimum, Maximum))
                {
                    throw new InvalidOperationException();
                }

                base.OnActionExecuting(context);
            }
            catch (InvalidOperationException ex)
            {
                context.Result = new BadRequestObjectResult(ex);
            }

        }

        public Version Minimum { get; }
        public Version Maximum { get; }

    }
}
