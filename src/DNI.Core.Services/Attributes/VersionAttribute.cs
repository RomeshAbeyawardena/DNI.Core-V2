using DNI.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = DNI.Core.Domains.Version;

namespace DNI.Core.Services.Attributes
{
    public sealed class VersionAttribute : ActionFilterAttribute, IActionConstraint
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
            var exceptionHandler = context.HttpContext.RequestServices.GetRequiredService<IExceptionHandler>();

            exceptionHandler.Try(() =>
            {
                if (!context.RouteData.Values.TryGetValue("version", out var versionString))
                {
                    throw new NullReferenceException("Version could not be obtained by the route data, ensure the {version} is specified somewhere within the MVC routing configuration");
                }

                if (!Version.TryParse(versionString.ToString(), out var version))
                {
                    throw new FormatException($"Version {versionString} string could not be parsed, expecting the semantic version format [major].[minor]");
                }

                if (!Version.IsInRange(version, Minimum, Maximum))
                {
                    throw new ArgumentOutOfRangeException($"Version {version} does not meet the specific range. Minimum: {Minimum} Maximum: {Maximum}");
                }

                base.OnActionExecuting(context);
            }, exception =>
            {
                context.Result = new BadRequestObjectResult(exception);
            }, typeDefinition => typeDefinition
                                    .GetType<NullReferenceException>()
                                    .GetType<FormatException>()
                                    .GetType<ArgumentOutOfRangeException>());

        }

        public bool Accept(ActionConstraintContext context)
        {
            var suitableCandidates = context.Candidates
                .Where(candidate => candidate.Constraints.Any(constraint => constraint is VersionAttribute));

            if (!suitableCandidates.Any())
            {
                return true;
            }

            if (!context.RouteContext.RouteData.Values.TryGetValue("version", out var versionString))
            {
                throw new InvalidOperationException("Version could not be obtained by the route data, ensure the {version} is specified somewhere within the MVC routing configuration");
            }

            if (!Version.TryParse(versionString.ToString(), out var version))
            {
                throw new FormatException($"Version {versionString} string could not be parsed, expecting the semantic version format [major].[minor]");
            }

            return Version.IsInRange(version, Minimum, Maximum);
        }

        public Version Minimum { get; }
        public Version Maximum { get; }

    }
}
