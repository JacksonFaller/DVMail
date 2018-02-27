using System.Windows;
using Mail.Client.WPF.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace Mail.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : Window
    {
        public StartView()
        {   
            InitializeComponent();
            this.DataContext = new StartViewModel(this);
        }
    }
}
