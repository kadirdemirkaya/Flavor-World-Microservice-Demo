using AuthenticationService.Test.Dtos;
using System.Text;

namespace AuthenticationService.Test
{
    public class ServiceTest
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public async Task user_register_test()
        {
            string apiBaseUrl = "https://localhost:5285";
            string createUserEndpoint = "User/Register/Create-User";
            CreateUserDto createUserDto = new CreateUserDto
            {
                FullName = "John",
                Email = "john@example.com",
                Password = "John123",
            };

            bool result = await CreateUserAsync(apiBaseUrl, createUserEndpoint, createUserDto);

            if (result)
            {
               
            }
            else
            {

            }
        }

        [Test]
        public async Task user_login_test()
        {
            string apiBaseUrl = "https://localhost:5285";
            string loginUserEndpoint = "User/Login/Login-User";
            LoginUserDto loginUserDto = new LoginUserDto
            {
                Email = "john@example.com",
                Password = "John123"
            };

            LoginUserCommandResponse response = await LoginUserAsync(apiBaseUrl, loginUserEndpoint, loginUserDto);

            if (response.IsSuccess)
            {

            }
            else
            {

            }
        }

        public async Task<bool> CreateUserAsync(string baseUrl, string endpoint, CreateUserDto createUserDto)
        {
            using (HttpClient client = new HttpClient())
            {
                
                string url = $"{baseUrl}/{endpoint}";
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(createUserDto);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

              
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public async Task<LoginUserCommandResponse> LoginUserAsync(string baseUrl, string endpoint, LoginUserDto loginUserDto)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{baseUrl}/{endpoint}";
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(loginUserDto);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client.PostAsync(url, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    LoginUserCommandResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUserCommandResponse>(responseContent);
                    return response;
                }
                else
                {
                    return new LoginUserCommandResponse(false, null);
                }
            }
        }
    }
}