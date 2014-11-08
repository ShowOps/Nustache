using Newtonsoft.Json;
using Nustache.Mvc5.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nustache.Mvc5
{
    public class Wax
    {
        protected WaxSection _WaxSection;

        public Wax()
        {
            _WaxSection = (WaxSection)ConfigurationManager.GetSection("wax");
        }

        public void AdditionalProcessing(object sender, CreatingViewArgs args)
        {
            // Only proceed if we are in debug mode.  There is no sense writing this json if we are in release mode.
            if (HttpContext.Current.IsDebuggingEnabled && !_WaxSection.Disabled)
            {
                var data = args.ControllerContext.Controller.ViewData.Model;

                var templatePath = args.ControllerContext.HttpContext.Server.MapPath(args.ViewPath);
                var templateName = System.IO.Path.GetFileNameWithoutExtension(templatePath);

                // clean up the path for the front end guys.  ie:  remove the leading ~
                var modifiedViewPath = args.ViewPath.Substring(1); // Skip the first character

                // LETS WRITE SOME JSON
                WriteJSON(args.ControllerContext.HttpContext, modifiedViewPath, templateName, data);
            }
        }

        protected void WriteJSON(HttpContextBase context, string viewPath, string templateName, object model)
        {
            var waxJson = JsonConvert.SerializeObject(
                model,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );

            var waxPath = _WaxSection.OutputPath;

            if (!System.IO.Directory.Exists(context.Server.MapPath(waxPath)))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath(waxPath));
            }

            var filePath = context.Server.MapPath(string.Format("{0}{1}.json", waxPath, templateName));

            if (ShouldWriteJson(context, filePath))
            {
                System.IO.File.WriteAllText(filePath, waxJson, System.Text.Encoding.UTF8);
            }
        }

        protected bool ShouldWriteJson(HttpContextBase context, string filePath)
        {
            // Check if a force flag has been set
            var forceFlag = context.Request.QueryString.AllKeys.Any(x => x != null && x.ToLower() == "force");

            var lastModified = System.IO.File.GetLastWriteTime(filePath);
            var timespan = DateTime.Now - lastModified;

            return timespan.TotalMinutes > _WaxSection.ExpiresInMinutes || forceFlag;
        }
    }
}
