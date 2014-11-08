using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nustache.Mvc5
{
    public class CreatingViewArgs
    {
        public ControllerContext ControllerContext { get; set; }
        public string ViewPath { get; set; }
        public string MasterPath { get; set; }
    }
}
