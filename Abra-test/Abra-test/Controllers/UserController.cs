using Abra_test.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Abra_test.Controllers {


    [ApiController]
    [Route("api/user")]
    public class UserController : Controller {
        private static Dictionary<string, UserModel> _users = new Dictionary<string, UserModel>();
        private readonly HttpClient _httpClient;

        public UserController(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        [HttpGet("GetUsersData")]
        public async Task<IActionResult> GetUsersData([FromQuery] string gender) {
            String url = "https://randomuser.me/api/?results=10&gender=" + gender;
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode) {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet("GetMostPopularCountry")]
        public async Task<IActionResult> GetMostPopularCountry() {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/?results=5000");
            var responseContent = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseContent);

            var countries = json["results"]
                .Select(result => result["location"]["country"].ToString())
                .GroupBy(country => country)
                .ToDictionary(group => group.Key, group => group.Count());

            var mostPopularCountry = countries.OrderByDescending(pair => pair.Value).First().Key;


            return Ok(mostPopularCountry);

        }

        [HttpGet("GetListOfMails")]
        public async Task<IActionResult> GetListOfMails() {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/?results=30");
            var responseContent = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseContent);


            var emails = json["results"]
                .Select(result => result["email"].ToString())
                .ToList();


            return Ok(emails);

        }


        [HttpGet("GetTheOldestUser")]
        public async Task<IActionResult> GetTheOldestUser() {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/?results=100");
            var responseContent = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseContent);

            var oldestUser = json["results"]
             .Select(result => new {
                 Name = result["name"]["first"].ToString() + " " + result["name"]["last"].ToString(),
                 Age = result["dob"]["age"].ToObject<int>()
             })
                .OrderByDescending(user => user.Age)
              .FirstOrDefault();


            return Ok(oldestUser);

        }


        [HttpPost("CreateNewUser")]
        public async Task<IActionResult> CreateNewUser(UserModel user) {
            if (_users.ContainsKey(user.Id.ToString())) {
                return BadRequest("User with this ID already exists");
            }
            else {
                _users.Add(user.Id.ToString(), user);
                return Ok(user);
            }
        }

        [HttpGet("GetNewUser/{id}")]
        public async Task<IActionResult> GetNewUser(string id) {
            if (_users.TryGetValue(id, out UserModel user)) {
                return Ok(user);
            }
            else {
                return NotFound();
            }
        }

        [HttpPatch("UpdateUserData/{id}")]
        public async Task<IActionResult> UpdateUserData(string id, [FromBody] UserModel updatedUser) {
            if (!_users.ContainsKey(id)) {
                return NotFound();
            }

            UserModel user = _users[id];

            if (!string.IsNullOrWhiteSpace(updatedUser.Name)) {
                user.Name = updatedUser.Name;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Email)) {
                user.Email = updatedUser.Email;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.DateOfBirth)) {
                user.DateOfBirth = updatedUser.DateOfBirth;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Phone)) {
                user.Phone = updatedUser.Phone;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Country)) {
                user.Country = updatedUser.Country;
            }


            return Ok(user);
        }

    }
}
