using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.Client.WPF.ViewModel
{
    class InboxLetterViewModel
    {
        public InboxLetterViewModel(Letter letter)
        {
            Letter = letter;
        }

        public Letter Letter { get; }
    }
}
