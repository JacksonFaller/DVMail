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
    /// Contoller to work with letters
    /// </summary>
    [ExceptionHandling]
    public class LettersController : ApiController
    {
        private const string ConnectionString =
            @"Data Source=JACKSONFALLERPC\SQLEXPRESS;Initial Catalog=MailDB;Integrated Security=True";
        private readonly ILettersRepository _lettersRepository = new LettersRepository(ConnectionString);

        /// <summary>
        /// Create new letter
        /// </summary>
        /// <param name="letter">letter model</param>
        /// <returns>updated letter model</returns>
        [HttpPost]
        [Route("api/letters/new")]
        public Letter CreateLetter([FromBody]Letter letter)
        {
            return _lettersRepository.CreateLetter(letter);
        }

        /// <summary>
        /// Get user's inbox
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>all letters addressed ot user with given id</returns>
        [HttpGet]
        [Route("api/letters/inbox/{userId}")]
        public IEnumerable<Letter> GetUsersInbox(Guid userId)
        { 
            return _lettersRepository.GetUsersInbox(userId);
        }

        /// <summary>
        /// Get user's sent mail
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>all letters sent by user with given id</returns>
        [HttpGet]
        [Route("api/letters/sentMail/{userId}")]
        public IEnumerable<Letter> GetUsersSentMail(Guid userId)
        {
            return _lettersRepository.GetUsersSentMail(userId);
        }

        /// <summary>
        /// Mark letter as read
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="letterId">letter id</param>
        [HttpPut]
        [Route("api/letters/inbox/{userId}/markAsRead/{letterId}")]
        public void MarkAsRead(Guid userId, Guid letterId)
        {
            _lettersRepository.SetIsRead(letterId, userId, true);
        }

        /// <summary>
        /// Mark letter as new
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="letterId">letter id</param>
        [HttpPut]
        [Route("api/letters/inbox/{userId}/markAsNew/{letterId}")]
        public void MarkAsNew(Guid userId, Guid letterId)
        {
            _lettersRepository.SetIsRead(letterId, userId, false);
        }

        /// <summary>
        /// Delete letter from inbox
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="letterId">letter id</param>
        [HttpDelete]
        [Route("api/letters/inbox/{userId}/delete/{letterId}")]
        public void DeleteInboxLetter(Guid userId, Guid letterId)
        {
            _lettersRepository.DeleteInboxLetter(letterId, userId);
        }

        /// <summary>
        /// Delete letter from sent mail
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="letterId">letter id</param>
        [HttpDelete]
        [Route("api/letters/sentMail/{userId}/delete/{letterId}")]
        public void DeleteSentMailLetter(Guid userId, Guid letterId)
        {
            _lettersRepository.DeleteSentMailLetter(letterId, userId);
        }
    }
}
