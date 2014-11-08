using Nustache.Core;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Nustache.Mvc5
{
    public delegate void CreatingViewHandler(object sender, CreatingViewArgs e);

    public class NustacheViewEngine : VirtualPathProviderViewEngine
    {
        public event CreatingViewHandler CreatingView;

        public NustacheViewEngine(string[] fileExtensions = null)
        {
            // If we're using MVC, we probably want to use the same encoder MVC uses.
            Encoders.HtmlEncode = HttpUtility.HtmlEncode;

            FileExtensions = fileExtensions ?? new[] { "mustache" };
            SetLocationFormats();
            RootContext = NustacheViewEngineRootContext.Model;
        }

        private void SetLocationFormats()
        {
            var _MasterLocationFormats = new List<string>();
            var _ViewLocationFormats = new List<string>();
            var _PartialViewLocationFormats = new List<string>();
            var _AreaMasterLocationFormats = new List<string>();
            var _AreaViewLocationFormats = new List<string>();
            var _AreaPartialViewLocationFormats = new List<string>();

            foreach (var fileExtension in FileExtensions)
            {

                _MasterLocationFormats.AddRange(new[]
                {
                    "~/Views/{1}/{0}." + fileExtension,
                    "~/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
                _ViewLocationFormats.AddRange(new[]
                {
                    "~/Views/{1}/{0}." + fileExtension,
                    "~/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
                _PartialViewLocationFormats.AddRange(new[]
                {
                    "~/Views/{1}/{0}." + fileExtension,
                    "~/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
                _AreaMasterLocationFormats.AddRange(new[]
                {
                    "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                    "~/Areas/{2}/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
                _AreaViewLocationFormats.AddRange(new[]
                {
                    "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                    "~/Areas/{2}/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
                _AreaPartialViewLocationFormats.AddRange(new[]
                {
                    "~/Areas/{2}/Views/{1}/{0}." + fileExtension,
                    "~/Areas/{2}/Views/Shared/{0}." + fileExtension,
                    "~/Views/Partials/{0}." + fileExtension
                });
            }

            MasterLocationFormats = _MasterLocationFormats.ToArray();
            ViewLocationFormats = _ViewLocationFormats.ToArray();
            PartialViewLocationFormats = _PartialViewLocationFormats.ToArray();
            AreaMasterLocationFormats = _AreaMasterLocationFormats.ToArray();
            AreaViewLocationFormats = _AreaViewLocationFormats.ToArray();
            AreaPartialViewLocationFormats = _AreaPartialViewLocationFormats.ToArray();
        }

        public NustacheViewEngineRootContext RootContext { get; set; }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            OnCreatingView(controllerContext, viewPath, masterPath);
            return GetView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return GetView(controllerContext, partialPath, null);
        }

        private IView GetView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new NustacheView(this, controllerContext, viewPath, masterPath);
        }

        protected virtual void OnCreatingView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            if (CreatingView != null)
            {
                var args = new CreatingViewArgs()
                {
                    ControllerContext = controllerContext,
                    MasterPath = masterPath,
                    ViewPath = viewPath
                };

                CreatingView(this, args);
            }
        }
    }

    public enum NustacheViewEngineRootContext
    {
        ViewData,
        Model
    }
}
