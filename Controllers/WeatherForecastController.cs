using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Multilingual.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public List<KeyValuePair<string, string>> MultilingualData { get; set; }

        public readonly object lockObject = new object();
        private Dictionary<string, string> resData = new Dictionary<string, string>();

        public TranslationController()
        {
            this.MultilingualData = new List<KeyValuePair<string, string>>(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("MenuHome", "Home"),
                new KeyValuePair<string, string>("MenuAbout", "About"),
                new KeyValuePair<string, string>("MenuServices", "Services"),
                new KeyValuePair<string, string>("MenuContact", "Contact"),
                new KeyValuePair<string, string>("MenuTeam", "Team"),
                new KeyValuePair<string, string>("MenuBlog", "Blog"),
                new KeyValuePair<string, string>("HeaderTitle", "FLexStart"),
                new KeyValuePair<string, string>("HomeTitle", "We offer modern solutions for growing your business"),
                new KeyValuePair<string, string>("HomeSubTitle", "We are team of talented designers making websites with Bootstrap"),
                new KeyValuePair<string, string>("ButtonGetStarted", "Get Started"),
                new KeyValuePair<string, string>("HomeWHOWEARETitle", "WHO WE ARE"),
                new KeyValuePair<string, string>("HomeHappyClients", "Happy Clients"),
                new KeyValuePair<string, string>("MenuProjects", "Projects"),
                new KeyValuePair<string, string>("MenuHoursOfSupport", "Hours Of Support"),
                new KeyValuePair<string, string>("MenuHardWorkers", "Hard Workers"),
                new KeyValuePair<string, string>("MenuFEATURESTitle", "FEATURES"),
                new KeyValuePair<string, string>("MenuSERVICESTitle", "SERVICES"),

                new KeyValuePair<string, string>("PortfolioPortfolioDetails", "Portfolio Details"),
                new KeyValuePair<string, string>("PortfolioProjectinformation", "Project information"),
                new KeyValuePair<string, string>("PortfolioCategory", "Category"),
                new KeyValuePair<string, string>("PortfolioClient", "Client"),
                new KeyValuePair<string, string>("PortfolioProjectURL", "Project URL"),
                new KeyValuePair<string, string>("PortfolioOurNewsletterHeading", "Our Newsletter"),
                new KeyValuePair<string, string>("PortfolioProjectdate", "Project date"),
                new KeyValuePair<string, string>("PortfolioTitle", "This is an example of portfolio detail")
            });
        }

        [HttpGet("GetLanguages")]
        public async Task<object> GetLanguages()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://translation.googleapis.com/language/translate/v2/languages?key=<<GOOGLE_API_CLIENT_ID>>"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadFromJsonAsync<LanguageData>();
                return (object)body;
            }
        }

        [HttpGet("{language}")]
        public async Task<object> Get(string language)
        {
            var client = new HttpClient();
            var tasks = new List<Task>();
            foreach (var tuple in this.MultilingualData)
            {
                tasks.Add(Task.Run(() => GetTranslations(language, client, tuple)));
            }
            Task.WaitAll(tasks.ToArray());

            return new
            { 
                data = resData
            };

        }

        private async Task GetTranslations(string language, HttpClient client, KeyValuePair<string, string> tuple)
        {
            if(language != "en")
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://translation.googleapis.com/language/translate/v2?key=" + "<<GOOGLE_API_CLIENT_ID>>"),
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "q", tuple.Value},
                        { "target", language },
                        { "source", "en" },
                        { "format", "html" }
                    }),
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadFromJsonAsync<LanguageTranslatedData>();
                    lock (lockObject)
                    {
                        resData.Add(tuple.Key, body.Data.Translations[0].translatedText);
                    }
                }
            }
            else
            {
                lock (lockObject)
                {
                    resData.Add(tuple.Key, tuple.Value);
                }
            }
            
        }
    }
}
