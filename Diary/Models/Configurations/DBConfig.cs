using Diary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Configurations
{
    public static class DBConfig
    {
        public static string Server
        {
            get
            {
                return Settings.Default.Server;
            }
            set
            {
                Settings.Default.Server = value;
            }
        }
        public static string Database
        {
            get
            {
                return Settings.Default.Database;
            }
            set
            {
                Settings.Default.Database = value;
            }
        }
        public static string Id
        {
            get
            {
                return Settings.Default.Id;
            }
            set
            {
                Settings.Default.Id = value;
            }
        }
        public static string Password
        {
            get
            {
                return Settings.Default.Password;
            }
            set
            {
                Settings.Default.Password = value;
            }
        }

    }
}
