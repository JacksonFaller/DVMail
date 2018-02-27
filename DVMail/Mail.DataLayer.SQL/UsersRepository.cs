using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.DataLayer.SQL
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;
        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUser(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * from Users where Name = @name";
                    command.Parameters.AddWithValue("@name", name);
                    using (var reader = command.ExecuteReader())
                    {
                        if(!reader.Read())
                            throw new ArgumentException($"User with name: {name} not found");

                        return new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = name,
                            //Password = reader.GetString(reader.GetOrdinal("Password"))
                        };
                    }
                }
            }
        }

        public User ValidateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Password = user.Password.GetHashCode().ToString();
                    command.CommandText = "Select * from Users where Name = @name and Password = @password";
                    command.Parameters.AddWithValue("@name", user.Name);
                    command.Parameters.AddWithValue("@password", user.Password);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException("Wrong username or password");

                        user.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                    }
                    return user;
                }
            }
        }

        public User CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();
                    user.Password = user.Password.GetHashCode().ToString();

                    command.CommandText = "insert into Users values (@id, @name, @password)";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@name", user.Name);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.ExecuteNonQuery();
                    
                    return user;
                }
            }
        }

        public void DeleteUser(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Delete from Users where Id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
