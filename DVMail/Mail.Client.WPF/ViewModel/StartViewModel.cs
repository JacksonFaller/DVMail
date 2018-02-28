using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mail.Client.WPF.View;
using Mail.Model;
using MessageBox = System.Windows.MessageBox;

namespace Mail.Client.WPF.ViewModel
{
    public class StartViewModel : INotifyPropertyChanged
    {
        private readonly ServiceClient _serviceClient;
        public User CurrentUser { get; set; }

        private readonly Window _window;

        public StartViewModel(Window window)
        {
            _serviceClient = new ServiceClient(ConfigurationManager.AppSettings["api"]);
            _name = string.Empty;
            _window = window;
        }

        public bool IsInputDataValid(string userName, string userPassword)
        {
            string message;
            if (userName.Length == 0)
            {
                message = "Необходимо задать имя пользователя";
            }
            else if (userPassword.Length == 0)
            {
                message = "Необходимо задать пароль";
            }

            else if (userName.Length < 5 || userName.Length > 25)
            {
                message = "Минимальная длина имени - 5, а максимальная - 25 символов";
            }

            else if (userPassword.Length < 5 || userPassword.Length > 25)
            {
                message = "Минимальная длина пароля - 5, а максимальная - 25 символов";
            }
            else return true;
            MessageBox.Show(message, "Внимание");
            return false;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private bool _isLocked;

        public bool IsLocked
        {
            get { return _isLocked; }
            set { _isLocked = value; OnPropertyChanged(nameof(IsLocked)); }
        }

        public ICommand SignInCommand
        {
            get
            {
                return new DelegateCommand(async o =>
                {
                    try
                    {
                        if (IsLocked) return;
                        IsLocked = true;
                        string password = GetPassword(o);
                        if (!IsInputDataValid(_name, password)) return;
                        CurrentUser = await _serviceClient.ValidateUser(new User(_name, password));
                        new MainView(CurrentUser).Show();
                        _window?.Close();
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        IsLocked = false;
                    }
                });
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return new DelegateCommand(async o =>
                {
                    try
                    {
                        if (IsLocked) return;
                        IsLocked = true;
                        string password = GetPassword(o);
                        if (!IsInputDataValid(_name, password)) return;
                        CurrentUser = await _serviceClient.CreateUser(new User(_name, password));
                        new MainView(CurrentUser).Show();
                        _window?.Close();
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        IsLocked = false;
                    }
                });
            }
        }

        private string GetPassword(object obj)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox == null)
                throw new ArgumentException("Expected type PasswordBox", nameof(obj));

            return passwordBox.Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
