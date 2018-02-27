using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.DataLayer
{
    public interface IUsersRepository
    {
        User GetUser(string name);
        User ValidateUser(User user);
        User CreateUser(User user);
        void DeleteUser(Guid id);
    }
}
