using System;
using System.Windows;
using System.Windows.Input;
using Mail.Client.WPF.ViewModel;
using Mail.Model;

namespace Mail.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(User user)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user);
        }
    }
}
