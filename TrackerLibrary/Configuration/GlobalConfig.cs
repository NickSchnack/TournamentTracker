using System;
using System.Configuration;
using System.Collections.Generic;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary.Configuration
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnection(StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.MySql:
                    MySqlConnector sqlConnector = new MySqlConnector();
                    Connection = sqlConnector;
                    break;

                case StorageType.TextFile:
                    TextConnector textConnector = new TextConnector();
                    Connection = textConnector;
                    break;
            }
        }

        public static String GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

