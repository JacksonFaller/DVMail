using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mail.Model;

namespace Mail.Client.WPF
{
    public class ServiceClient
    {
        private readonly HttpClient _client;

        public ServiceClient(string connectionString)
        {
            _client = new HttpClient {BaseAddress = new Uri(connectionString)};
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private T ResponseParse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);

            return response.Content.ReadAsAsync<T>().Result;
        }

        public async Task<User> ValidateUser(User user)
        {
            var response = await _client.PostAsJsonAsync("users/", user);
            return ResponseParse<User>(response);
        }

        public async Task<User> GetUser(string name)
        {
            var response = await _client.GetAsync($"users/{name}");
            return ResponseParse<User>(response);
        }

        public async Task<IEnumerable<Letter>> GetUsersInbox(Guid userId)
        {
            var response = await _client.GetAsync($"letters/inbox/{userId}");
            return ResponseParse<IEnumerable<Letter>>(response);
        }

        public async Task<IEnumerable<Letter>> GetUsersSentMail(Guid userId)
        {
            var response = await _client.GetAsync($"letters/sentMail/{userId}");
            return ResponseParse<IEnumerable<Letter>>(response);
        }

        public async Task<User> CreateUser(User user)
        {
            var response = await _client.PostAsJsonAsync("users/new", user);
            return ResponseParse<User>(response);
        }

        public async Task<Letter> SendLetter(Letter letter)
        {
            var response = await _client.PostAsJsonAsync("letters/new", letter);
            return ResponseParse<Letter>(response);
        }

        public string DeleteInboxLetter(Guid userId, Guid letterId)
        {
            var response = _client.DeleteAsync($"letters/inbox/{userId}/delete/{letterId}").Result;
            return ResponseParse<string>(response);
        }
        public string DeleteSentMailLetter(Guid userId, Guid letterId)
        {
            var response = _client.DeleteAsync($"letters/sentMail/{userId}/delete/{letterId}").Result;
            return ResponseParse<string>(response);
        }

        public void MarkAsRead(Guid userId, Guid letterId)
        {
            _client.PutAsJsonAsync<string>($"letters/inbox/{userId}/markAsRead/{letterId}", null);
        }

        public void MarkAsNew(Guid userId, Guid letterId)
        {
            _client.PutAsJsonAsync<string>($"letters/inbox/{userId}/markAsNew/{letterId}", null);
        }
    }
}
