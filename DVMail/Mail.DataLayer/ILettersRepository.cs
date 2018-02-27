using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.DataLayer
{
    public interface ILettersRepository
    {
        Letter CreateLetter(Letter letter);
        IEnumerable<Letter> GetUsersInbox(Guid userId);
        IEnumerable<Letter> GetUsersSentMail(Guid userId);
        void SetIsRead(Guid letterId, Guid userId, bool isRead);
        void DeleteLetter(Guid id);
        void DeleteInboxLetter(Guid letterId, Guid userId);
        void DeleteSentMailLetter(Guid letterId, Guid userId);
    }
}
