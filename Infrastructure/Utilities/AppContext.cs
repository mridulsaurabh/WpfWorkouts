using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public class AppContext
    {
        public AppContext()
        {
            Assembly oAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo oFileVersionInfo = FileVersionInfo.GetVersionInfo(oAssembly.Location);
            this.AppName = oFileVersionInfo.ProductName;
            this.AppVersion = "v" + oFileVersionInfo.ProductMajorPart + "." + oFileVersionInfo.ProductMinorPart + "." + oFileVersionInfo.ProductBuildPart + "." + oFileVersionInfo.ProductPrivatePart;
            this.UserName = Environment.UserName;
            this.UserDomain = Environment.UserDomainName;
        }


        public string AppName { get; set; }

        public string AppVersion { get; set; }

        public string UserName { get; set; }

        public string UserDomain { get; set; }
    }
}
