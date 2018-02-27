using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mail.Model
{
    public class Letter
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }

        public Guid AddresserId { get; set; }
        public string AddresserName { get; set; }
        public IEnumerable<Guid> AddresseeId { get; set; }
        public IEnumerable<string> AddresseeName { get; set; }

        public Letter()
        {
        }

        public Letter(string subject, string text, Guid addresser, IEnumerable<Guid> addressee)
        {
            Subject = subject;
            Text = text;
            AddresseeId = addressee;
            AddresserId = addresser;
        }
    }
}
