using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp
{

    //public class VersionAttribute : ActionFilterAttribute
    //{
    //    public VersionAttribute(string minimum, string maximum = null)
    //    {
    //        Minimum = ParseVersion(minimum);

    //        if(maximum != null)
    //        { 
    //            Maximum = ParseVersion(maximum);
    //        }
    //    }

    //    public Tuple<int, int> Minimum { get; }
    //    public Tuple<int, int> Maximum { get; }

    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        if (!context.RouteData.Values.TryGetValue("version", out var version))
    //        {
    //            context.Result = new BadRequestResult();
    //            return;
    //        }

    //        var parsedVersion = ParseVersion(version.ToString());

    //        if(Maximum == null)
    //        {
    //            //Maximum version not supplied - ensure current version is equal than or greater than requested version...
    //            if(Minimum.Item1 < parsedVersion.Item1 && Minimum.Item2 < parsedVersion.Item2)
    //            {
    //                context.Result = new BadRequestResult();
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            if(Minimum.Item1 < parsedVersion.Item1 || Minimum.Item2 < parsedVersion.Item2
    //                && Maximum.Item1 > parsedVersion.Item1 || Maximum.Item2 > parsedVersion.Item2)
    //            {
    //                context.Result = new BadRequestResult();
    //                return;
    //            }
    //        }

    //        base.OnActionExecuting(context);
    //    }

    //}
}
