using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mail.Client.WPF.ViewModel;
using Mail.Model;

namespace Mail.Client.WPF.View
{
    /// <summary>
    /// Interaction logic for InboxLetterView.xaml
    /// </summary>
    public partial class SentMailView : Window
    {
        public SentMailView(Letter letter)
        {
            InitializeComponent();
            DataContext = new SentMailViewModel(letter);
        }
    }
}
