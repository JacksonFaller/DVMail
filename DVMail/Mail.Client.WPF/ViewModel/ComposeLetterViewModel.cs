using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Mail.Model;
using static System.String;

namespace Mail.Client.WPF.ViewModel
{
    class ComposeLetterViewModel : INotifyPropertyChanged
    {
        private readonly ServiceClient _serviceClient;
        private readonly User _currentUser;
        public event PropertyChangedEventHandler PropertyChanged;

        public ComposeLetterViewModel(User currentUser)
        {
            _serviceClient = new ServiceClient(ConfigurationManager.AppSettings["api"]);
            _currentUser = currentUser;
        }

        private ObservableCollection<string> _addressee = new ObservableCollection<string>();
        public ObservableCollection<string> Addressee
        {
            get { return _addressee; }
            set
            {
                _addressee = value;
                OnPropertyChanged(nameof(Addressee));
            }
        }

        private readonly List<Guid> _addresseeId = new List<Guid>(8);

        private string _addresseeText;
        public string AddresseeText
        {
            get { return _addresseeText; }
            set
            {
                _addresseeText = value;
                OnPropertyChanged(nameof(AddresseeText));
            }
        }

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        private string _body;
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                OnPropertyChanged(nameof(Body));
            }
        }

        public ICommand SendLetterCommand
        {
            get
            {
                return new DelegateCommand(async o =>
                {
                    try
                    {
                        if (!ValidateData())
                        {
                            MessageBox.Show("Необходимо заполнить все поля", "Внимание", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                            return;
                        }
                        
                        var letter = new Letter(_subject, _body, _currentUser.Id, _addresseeId);
                        await _serviceClient.SendLetter(letter);
                        var window = o as Window;
                        window?.Close();
                    }
                    catch (HttpRequestException e)
                    {
                        MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public ICommand AddAddresseeCommand
        {
            get
            {
                return new DelegateCommand(async o =>
                {
                    try
                    {
                        if (IsNullOrWhiteSpace(_addresseeText)) return;
                        _addresseeId.Add((await _serviceClient.GetUser(_addresseeText)).Id);
                        //Addressee = new ObservableCollection<string>(Addressee);
                        Addressee.Add(_addresseeText);
                        AddresseeText = Empty;
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public ICommand DeleteAddresseeCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    try
                    {
                        var combo = o as ComboBox;
                        if(combo == null) return;
                        if(combo.SelectedIndex < 0) return;
                        _addresseeId.RemoveAt(combo.SelectedIndex);
                        Addressee.Remove(combo.Text);
                        combo.SelectedIndex = 0;
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public bool ValidateData()
        {
            return !(_addresseeId.Count == 0 || IsNullOrWhiteSpace(_subject) || IsNullOrWhiteSpace(_body));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
