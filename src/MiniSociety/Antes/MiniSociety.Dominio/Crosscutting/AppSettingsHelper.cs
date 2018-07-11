using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSociety.Dominio.Crosscutting
{
    public class AppSettingsHelper
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _appConfiguration;

        public AppSettingsHelper(Microsoft.Extensions.Configuration.IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public string GetConnectionString(string name = "DefaultConnection")
            => _appConfiguration[$"ConnectionStrings:{name}"];

        public string GetSetting(string name) => _appConfiguration[$"{name}"];
    }
}
