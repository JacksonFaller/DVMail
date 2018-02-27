using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.DataLayer.SQL.Tests
{
    static class MainTest
    {
        public const string ConnectionString = 
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=MailDB;Integrated Security=True";
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static readonly Random Random = new Random();

        public static string GetRandomString(int length)
        {
            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = Chars[Random.Next(Chars.Length)];
            }
            return new string(stringChars);
        }

        public static Letter GetRandomLetter(Guid addresser, Guid addressee)
        {
            string subject = GetRandomString(20);
            string text = GetRandomString(50);
            return new Letter(subject, text, addresser, addressee.Yield());
        }

        public static User GetRandomUser()
        {
            string name = GetRandomString(15);
            return new User(name, name.GetHashCode().ToString());
        }
    }
}