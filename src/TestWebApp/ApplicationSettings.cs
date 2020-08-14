
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApp
{
    public class ApplicationSettings
    {
        public ApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this, configuration => configuration.BindNonPublicProperties = true);
        }

        public string InitialVector { get; private set; }
        public string Key1 {get; private set; }
        public string Key2 {get; private set; }
        public string Key3 {get; private set; }
        public string Key4 {get; private set; }
        public string Salt {get; private set; }
    }
}
