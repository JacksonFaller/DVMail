using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mail.DataLayer;
using Mail.DataLayer.SQL;
using Mail.Model;

namespace Mail.API.Controllers
{
    /// <summary>
    /// Controller to work with users
    /// </summary>
    [ExceptionHandling]
    public class UsersController : ApiController
    {
        private const string ConnectionString = 
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=MailDB;Integrated Security=True";
        private readonly IUsersRepository _usersRepository = new UsersRepository(ConnectionString);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">user model</param>
        /// <returns>updated user model</returns>
        [HttpPost]
        [Route("api/users/new/")]
        public User CreateUser([FromBody]User user)
        {
            return _usersRepository.CreateUser(user);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="name">user's name</param>
        /// <returns>user model with given id</returns>
        [HttpGet]
        [Route("api/users/{name}")]
        public User GetUser(string name)
        {
            return _usersRepository.GetUser(name);
        }

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>user model</returns>
        [HttpPost]
        [Route("api/users/")]
        public User ValidateUser([FromBody] User user)
        {
            return _usersRepository.ValidateUser(user);
        }
    }
}
