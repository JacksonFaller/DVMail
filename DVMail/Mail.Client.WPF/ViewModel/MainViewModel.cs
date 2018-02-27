using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Mail.Client.WPF.View;
using Mail.Model;

namespace Mail.Client.WPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ServiceClient _serviceClient;

        private ObservableCollection<Letter> _inbox;
        private ObservableCollection<Letter> _sentMail;
        private readonly User _currentUser;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(User currentUser)
        {
            _serviceClient = new ServiceClient(ConfigurationManager.AppSettings["api"]);
            _currentUser = currentUser;
            UpdateInbox();
            UpdateSentMail();
        }

        private Letter _selectedInboxLetter;
        public Letter SelectedInboxLetter
        {
            get { return _selectedInboxLetter; }
            set
            {
                _selectedInboxLetter = value;
                OnPropertyChanged(nameof(SelectedInboxLetter));
            }
        }

        private Letter _selectedSentMailLetter;

        public Letter SelectedSentMailLetter
        {
            get { return _selectedSentMailLetter; }
            set
            {
                _selectedSentMailLetter = value;
                OnPropertyChanged(nameof(SelectedSentMailLetter));
            }
        }

        public ObservableCollection<Letter> Inbox
        {
            get { return _inbox; }
            set { _inbox = value; OnPropertyChanged(nameof(Inbox)); }
        }

        public ObservableCollection<Letter> SentMail
        {
            get { return _sentMail; }
            set { _sentMail = value; OnPropertyChanged(nameof(SentMail)); }
        }

        public ICommand DeselectInboxLettersCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    SelectedInboxLetter = null;
                });
            }
        }

        public ICommand DeselectSentMailLettersCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    SelectedSentMailLetter = null;
                });
            }
        }
       
        public ICommand ComposeLetterCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    var composeLetter = new ComposeLetterView(_currentUser);
                    composeLetter.ShowDialog();
                    UpdateSentMail();
                });
            }
        }

        public ICommand OpenInboxLetterCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    if (SelectedInboxLetter == null) return;
                    new InboxLetterView(SelectedInboxLetter).ShowDialog();
                    SelectedInboxLetter.IsRead = true;
                    Inbox = new ObservableCollection<Letter>(_inbox);
                    _serviceClient.MarkAsRead(_currentUser.Id, SelectedInboxLetter.Id);
                });

            }
        }

        public ICommand OpenSentMailLetterCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    if (SelectedSentMailLetter == null) return;
                    var letterView = new SentMailView(SelectedSentMailLetter);
                    letterView.ShowDialog();
                });

            }
        }

        public ICommand MarkAsNewCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    SelectedInboxLetter.IsRead = false;
                    Inbox = new ObservableCollection<Letter>(_inbox);
                    _serviceClient.MarkAsNew(_currentUser.Id, SelectedInboxLetter.Id);
                });
            }
        }

        public ICommand DeleteInboxLetterCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    if (SelectedInboxLetter == null) return;
                    _serviceClient.DeleteInboxLetter(_currentUser.Id, SelectedInboxLetter.Id);
                    UpdateInbox();
                });
            }
        }

        public ICommand DeleteSentMailLetterCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    if (SelectedSentMailLetter == null) return;
                    _serviceClient.DeleteSentMailLetter(_currentUser.Id, SelectedSentMailLetter.Id);
                    UpdateSentMail();
                });
            }
        }

        public ICommand UpdateInboxCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    UpdateInbox();
                });
            }
        }

        public ICommand UpdateSentMailCommand
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    UpdateSentMail();
                });
            }
        }
        public async void UpdateInbox()
        {
            var letters = (await _serviceClient.GetUsersInbox(_currentUser.Id)).ToList();
            Inbox = new ObservableCollection<Letter>(letters);
        }

        public async void UpdateSentMail()
        {
            var letters = await _serviceClient.GetUsersSentMail(_currentUser.Id);
            SentMail = new ObservableCollection<Letter>(letters);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
