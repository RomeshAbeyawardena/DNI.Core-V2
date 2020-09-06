﻿
using Microsoft.Extensions.Configuration;

namespace TestWebApp
{
    public class ApplicationSettings
    {
        public ApplicationSettings(IConfiguration configuration)
        {
            configuration.Bind(this, configuration => configuration.BindNonPublicProperties = true);
            DefaultConnectionString = configuration.GetConnectionString("default");
        }
        public string DefaultConnectionString { get; private set; }
        public string InitialVector { get; private set; }
        public string PersonalKey {get; private set; }
        public string CommonKey {get; private set; }
        public string SharedKey {get; private set; }
        public string GeneralKey {get; private set; }
        public string Salt {get; private set; }
    }
}
