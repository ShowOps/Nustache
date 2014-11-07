using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nustache.Mvc5.Config
{
    public class WaxSection : ConfigurationSection
    {
        [ConfigurationProperty("disabled", DefaultValue = "false", IsRequired = false)]
        public bool Disabled
        {
            get
            {
                return (bool)this["disabled"];
            }
            set
            {
                this["disabled"] = value;
            }
        }

        [ConfigurationProperty("outputPath", DefaultValue = "/wax/data/", IsRequired = false)]
        public string OutputPath
        {
            get
            {
                return (string)this["outputPath"];
            }
            set
            {
                // Clean it up
                if (!value.EndsWith("/"))
                {
                    value = string.Format("{0}/", value);
                }
                this["outputPath"] = value;
            }
        }

        [ConfigurationProperty("expiresInMinutes", DefaultValue = "10080", IsRequired = false)] // 1 week
        public int ExpiresInMinutes
        {
            get
            {
                return (int)this["expiresInMinutes"];
            }
            set
            {
                this["expiresInMinutes"] = value;
            }
        }
    }
}
