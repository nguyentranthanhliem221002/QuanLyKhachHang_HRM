using FE.Models;

namespace FE.Services
{
    public class TestService
    {
        private readonly HttpClient _client;
        public TestService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<QuestionViewModel>> GetQuestions(string subject)
        {
            var response = await _client.GetAsync($"api/Test/GetQuestions/{subject}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<QuestionViewModel>>();
        }

        public async Task<TestResultViewModel> SubmitTest(TestSubmitViewModel payload)
        {
            var response = await _client.PostAsJsonAsync("api/Test/Submit", payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TestResultViewModel>();
        }
    }


}
