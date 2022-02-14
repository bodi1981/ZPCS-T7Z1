using Diary.Commands;
using Diary.Models.Configurations;
using Diary.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class DBConfigVIewModel : ViewModelBase
    {
        private bool _isCheckedDBConfig;
        public DBConfigVIewModel(bool isCheckedDBConfig)
        {
            _isCheckedDBConfig = isCheckedDBConfig;
            SaveCommand = new RelayCommand(SaveDBConfig);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel(object obj)
        {
            if (_isCheckedDBConfig)
            {
                Application.Current.Shutdown();
            }

            CloseWindow(obj as Window);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public string Server
        {
            get
            {
                return DBConfig.Server;
            }
            set
            {
                DBConfig.Server = value;
                OnPropertyChanged();
            }
        }
        public string Database
        {
            get
            {
                return DBConfig.Database;
            }
            set
            {
                DBConfig.Database = value;
                OnPropertyChanged();
            }
        }
        public string Id
        {
            get
            {
                return DBConfig.Id;
            }
            set
            {
                DBConfig.Id = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get
            {
                return DBConfig.Password;
            }
            set
            {
                DBConfig.Password = value;
                OnPropertyChanged();
            }
        }

        private void SaveDBConfig(object obj)
        {
            Settings.Default.Save();
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentExecutablePath);
            Application.Current.Shutdown();
        }
        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
