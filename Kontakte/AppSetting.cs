using System;
using System.Configuration;

namespace KontakteApp
{
    public class AppSetting
    {
        Configuration config;

        public AppSetting()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetConnection(string key)
        {
            return config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public void SaveConnection(string key, string value)
        {
            config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";
            config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection(config.ConnectionStrings.SectionInformation.Name);
            Properties.Settings.Default.Reload();
        }
    }
}
