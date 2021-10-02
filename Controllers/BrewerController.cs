using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using brewerApi.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace brewerApi.Controllers
{
    [ApiController]
    [Route("brewers")]
    public class BrewerController: ControllerBase
    {
        private BrewerConfig _brewerConfig;
        public BrewerController(IOptions<BrewerConfig> config) {
            this._brewerConfig = config.Value;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<BrewerModel>>> GetBrewers([FromQuery]int per_page) {
            try {
                using(var httpClient = new HttpClient()) {
                    string contentType = "application/json";
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                    string p = per_page.ToString();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{this._brewerConfig.Url}?per_page=${p}");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    if(response.IsSuccessStatusCode) {
                        var json = await response.Content.ReadAsStringAsync();
                        List<BrewerModel> data = JsonConvert.DeserializeObject<List<BrewerModel>>(json);

                        return Ok(data);
                    } else {
                        return BadRequest(response);
                    }
                }
            } catch(Exception ex) {
                throw;
            }
        }
        
    }
}