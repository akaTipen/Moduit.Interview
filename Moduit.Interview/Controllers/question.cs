using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Moduit.Interview.Controllers
{
    public class Model
    {
        public int id { get; set; }
        public int category { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string footer { get; set; }
        public List<string> tags { get; set; }
        public List<Items> items { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class Items
    {
        public string title { get; set; }
        public string description { get; set; }
        public string footer { get; set; }
    }

    [Route("backend/[controller]")]
    [ApiController]
    public class questionController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> One()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://screening.moduit.id/backend/question/one");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var model = JsonConvert.DeserializeObject<Model>(jsonString);

                        return Ok(new
                        {
                            model.id,
                            model.title,
                            model.description,
                            model.footer,
                            model.createdAt
                        });
                    }
                    else
                        return NotFound(new { response.StatusCode });
                }
            }
            catch (Exception e)
            {
                return NotFound(new { e.Message });
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Two()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://screening.moduit.id/backend/question/two");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<Model>>(jsonString);
                        list = list
                                .Where(x => (x.description.Contains("Ergonomic") || x.title.Contains("Ergonomic")) && x.tags != null)
                                .Where(x => x.tags.Any(y => y.Contains("Sports")))
                                .OrderByDescending(x => x.id)
                                .Take(3).ToList();

                        return Ok(
                            list.Select(x => new
                            {
                                x.id,
                                x.title,
                                x.description,
                                x.footer,
                                x.tags,
                                x.createdAt,
                            })
                        );
                    }
                    else
                        return NotFound(new { response.StatusCode });
                }
            }
            catch (Exception e)
            {
                return NotFound(new { e.Message });
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Three()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://screening.moduit.id/backend/question/three");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        List<Model> model = JsonConvert.DeserializeObject<List<Model>>(jsonString);
                        List<Object> obj = new List<Object>();
                        foreach (var i in model)
                        {
                            if (i.items != null)
                            {
                                foreach (var j in i.items)
                                {
                                    obj.Add(new
                                    {
                                        i.id,
                                        i.category,
                                        j.title,
                                        j.description,
                                        j.footer,
                                        i.createdAt
                                    });
                                }
                            }
                        }
                        return Ok(obj);
                    }
                    else
                        return NotFound(new { response.StatusCode });
                }
            }
            catch (Exception e)
            {
                return NotFound(new { e.Message });
            }
        }
    }
}
