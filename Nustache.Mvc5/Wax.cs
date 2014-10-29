using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nustache.Mvc5
{
    public class Wax
    {
        public void AdditionalProcessing(object sender, AdditionalProcessingArgs args)
        {
            var data = args.ControllerContext.Controller.ViewData.Model;

            var templatePath = args.ControllerContext.HttpContext.Server.MapPath(args.ViewPath);
            var templateName = System.IO.Path.GetFileNameWithoutExtension(templatePath);

            // clean up the path for the front end guys.  ie:  remove the leading ~
            var modifiedViewPath = args.ViewPath.Substring(1); // Skip the first character

            // LETS WRITE SOME JSON
            WriteJSON(args.ControllerContext.HttpContext, modifiedViewPath, templateName, data);
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

            if (!System.IO.Directory.Exists(context.Server.MapPath("/wax/data/")))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath("/wax/data/"));
            }

            System.IO.File.WriteAllText(context.Server.MapPath("/wax/data/" + templateName + ".json"), waxJson, System.Text.Encoding.UTF8);
        }
    }
}
