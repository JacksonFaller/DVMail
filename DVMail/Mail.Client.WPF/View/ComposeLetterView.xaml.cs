using System.Windows;
using System.Windows.Documents;
using Mail.Client.WPF.ViewModel;
using Xceed.Wpf.Toolkit;
using Mail.Model;

namespace Mail.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for ComposeLetterView.xaml
    /// </summary>
    public partial class ComposeLetterView : Window
    {
        public ComposeLetterView(User currentUser)
        {
            InitializeComponent();
            DataContext = new ComposeLetterViewModel(currentUser);
        }
    }
}
