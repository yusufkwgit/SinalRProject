using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using SignalRWebUi.Dtos.MessageDto;

namespace SignalRWebUi.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index(string tableName = null)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:7195/api/Contact");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray item = JArray.Parse(responseBody);
            string value = item[0]["location"].ToString();
            ViewBag.location = value;

            if (!string.IsNullOrEmpty(tableName))
            {
                HttpClient tableClient = new HttpClient();
                string encodedTableName = Uri.EscapeDataString(tableName);
                var tableResponse = await tableClient.GetAsync($"https://localhost:7195/api/MenuTables/GetMenuTableIdByName/{encodedTableName}");
                if (tableResponse.IsSuccessStatusCode)
                {
                    string tableId = await tableResponse.Content.ReadAsStringAsync();
                    Response.Cookies.Append("MenuTableId", tableId, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddHours(2),
                        HttpOnly = false,
                        IsEssential = true
                    });
                }
            }

            return View();
        }

        [HttpGet]
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7195/api/Message", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Default");
            }
            return View();
        }
    }
}
