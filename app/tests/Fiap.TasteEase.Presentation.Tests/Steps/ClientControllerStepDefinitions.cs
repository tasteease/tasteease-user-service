using Fiap.TasteEase.Api.ViewModels;
using Fiap.TasteEase.Application.Ports;
using Fiap.TasteEase.Domain.Aggregates.ClientAggregate;
using Fiap.TasteEase.Presentation.Tests.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace Fiap.TasteEase.Presentation.Tests.Steps
{
    [Binding]
    public class ClientControllerStepDefinitions
    {
        private const string BaseAddress = "http://localhost/";

        public WebApplicationFactory<Program> Factory { get; }
        public HttpClient Client { get; set; } = null!;
        private HttpResponseMessage Response { get; set; } = null!;
        public JsonFilesRepository JsonFilesRepo { get; }
        private Client? Entity { get; set; }

        public ClientControllerStepDefinitions(
            WebApplicationFactory<Program> factory, 
            JsonFilesRepository jsonFilesRepo)
        {
            Factory = factory;
            JsonFilesRepo = jsonFilesRepo;
        }

        private JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true
        };

        [Given(@"a valid create client request")]
        public async Task GivenAValidCreateClientRequest()
        {
            Client = Factory.CreateDefaultClient(new Uri(BaseAddress));
        }

        [When(@"the POST request is sent to create a client with '(.*)' to '(.*)'")]
        public async Task WhenThePOSTRequestIsSentToCreateAClient(string file, string endpoint)
        {
            var json = JsonFilesRepo.Files[file];
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            Response = await Client.PostAsync(endpoint, content);
        }

        [Then(@"a (.*) Created response is returned")]
        public async Task ThenACreatedResponseIsReturned(int statusCode)
        {
            var expected = (HttpStatusCode)statusCode;
            var response = await Response.Content.ReadAsStringAsync();
            var responseViewModel = JsonSerializer.Deserialize<ResponseViewModel<Guid?>>(response, JsonSerializerOptions);

            if (responseViewModel.Error)
                Assert.False(responseViewModel.Error, string.Join(',', responseViewModel.ErrorMessages));

            Assert.Equal(expected, Response.StatusCode);
        }

        [Then(@"the response contains the client ID")]
        public async Task ThenTheResponseContainsTheClientID()
        {
            var response = await Response.Content.ReadAsStringAsync();
            var responseViewModel = JsonSerializer.Deserialize<ResponseViewModel<Guid?>>(response, JsonSerializerOptions);
            Assert.False(responseViewModel.Error);
        }
    }
}
