using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mail.DataLayer;
using Mail.Model;

namespace Mail.DataLayer.SQL
{
    public class LettersRepository : ILettersRepository
    {
        private readonly string _connectionString;

        public LettersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Letter GetLetter(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * from Letters where Id = @letterId";
                    command.Parameters.AddWithValue("@letterId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"CreateLetter with letterId: {id} not found");

                        return new Letter
                        {
                            Id = id,
                            AddresseeId = reader.GetGuid(reader.GetOrdinal("AddresseeId")).Yield(),
                            AddresserId = reader.GetGuid(reader.GetOrdinal("AddresserId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Text = reader.GetString(reader.GetOrdinal("Text")),
                            IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead"))
                        };
                    }
                }
            }
        }

        public IEnumerable<Letter> GetUsersInbox(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "Select LetterId, AddresseeId, AddresserId, IsRead, Subject, Date, Text, Name from (" +
                        "Select * from Inbox where AddresseeId = @userId) as A " +
                        "join Letters on A.LetterId = Letters.Id join Users on Users.Id = A.AddresserId order by Date desc";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                       while (reader.Read())
                        {
                            yield return new Letter
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("LetterId")),
                                AddresseeId = userId.Yield(),
                                AddresserId = reader.GetGuid(reader.GetOrdinal("AddresserId")),
                                AddresserName = reader.GetString(reader.GetOrdinal("Name")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Text = reader.GetString(reader.GetOrdinal("Text")),
                                IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead"))
                            };
                        }
                    }
                }
            }
        }

        public IEnumerable<Letter> GetUsersSentMail(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "Select LetterId, AddresseeId, AddresserId, Subject, Date, Text, Name from (" +
                        "Select * from SentMail where AddresserId = @userId) as A " +
                        "join Letters on A.LetterId = Letters.Id join Users on Users.Id = A.AddresseeId order by LetterId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        var lettersList = GetUsersSentMail(reader, userId).ToList();
                        if (lettersList.Count < 2 ) return lettersList;

                        var resultList = new List<Letter>(lettersList.Count);
                        
                        while (lettersList.Count != 0)
                        {
                            var curLetter = lettersList.First();

                            var collectLetter = (from us in lettersList.TakeWhile(x => x.Id == curLetter.Id)
                                select new {userID = us.AddresseeId.First(), userName = us.AddresseeName.First()}).ToList();

                            curLetter.AddresseeId = collectLetter.Select(x => x.userID);
                            curLetter.AddresseeName = collectLetter.Select(x => x.userName);

                            resultList.Add(curLetter);
                            lettersList.RemoveRange(0, collectLetter.Count());
                        }
                        return resultList;
                    }
                }
            }
        }

        private IEnumerable<Letter> GetUsersSentMail(SqlDataReader reader, Guid userId)
        {
            while (reader.Read())
            {
                yield return new Letter
                {
                    Id = reader.GetGuid(reader.GetOrdinal("LetterId")),
                    AddresserId = userId,
                    AddresseeId = reader.GetGuid(reader.GetOrdinal("AddresseeId")).Yield(),
                    AddresseeName = reader.GetString(reader.GetOrdinal("Name")).Yield(),
                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                    Subject = reader.GetString(reader.GetOrdinal("Subject")),
                    Text = reader.GetString(reader.GetOrdinal("Text"))
                };
            }
        }

        public void DeleteLetter(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Delete from Letters where Id = @letterId";
                    command.Parameters.AddWithValue("@letterId", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteInboxLetter(Guid letterId, Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Delete from Inbox where LetterId = @letterId and AddresseeId = @addresseeId";
                    command.Parameters.AddWithValue("@letterId", letterId);
                    command.Parameters.AddWithValue("@addresseeId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSentMailLetter(Guid letterId, Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Delete from SentMail where LetterId = @letterId and AddresserId = @addresserId";
                    command.Parameters.AddWithValue("@letterId", letterId);
                    command.Parameters.AddWithValue("@addresserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Letter CreateLetter(Letter letter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    letter.Id = Guid.NewGuid();
                    letter.IsRead = false;
                    letter.Date = DateTime.UtcNow;

                    command.CommandText = "insert into Letters values (@letterId, @subject, @date, @text)";
                    command.Parameters.AddWithValue("@letterId", letter.Id);
                    command.Parameters.AddWithValue("@subject", letter.Subject);
                    command.Parameters.AddWithValue("@date", letter.Date);
                    command.Parameters.AddWithValue("@text", letter.Text);
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    command.CommandText = "insert into Inbox values(@letterId, @addressee, @addresser, @isRead)";
                    command.Parameters.AddWithValue("@letterId", letter.Id);
                    command.Parameters.AddWithValue("@addresser", letter.AddresserId);
                    var isReadParam = command.Parameters.AddWithValue("@isRead", letter.IsRead);

                    var param = command.Parameters.Add(new SqlParameter("@addressee", SqlDbType.UniqueIdentifier));
                    foreach (var addressee in letter.AddresseeId)
                    {
                        command.Parameters["@addressee"].Value = addressee;
                        command.ExecuteNonQuery();
                    }

                    command.Parameters.Remove(isReadParam);
                    command.CommandText = "insert into SentMail values(@letterId, @addresser, @addressee)";

                    foreach (var addressee in letter.AddresseeId)
                    {
                        command.Parameters["@addressee"].Value = addressee;
                        command.ExecuteNonQuery();
                    }

                    return letter;
                }
            }
        }

        public void SetIsRead(Guid letterId, Guid userId, bool isRead)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Update Inbox set IsRead = @isRead where LetterId = @letterId and AddresseeId = @userId";
                    command.Parameters.AddWithValue("@isRead", isRead);
                    command.Parameters.AddWithValue("@letterId", letterId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}