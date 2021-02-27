using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TriumphantRTMP.Api.ViewModels;

namespace TriumphantRTMP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YoutubeController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string YOUTUBE_BASE_URL = "https://youtube.googleapis.com/youtube/v3/liveStreams";
        private readonly ILogger<YoutubeController> logger;
        public YoutubeController(HttpClient _httpClient, ILogger<YoutubeController> _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }

        [HttpPost]
        [Route("StartStreaming")]
        public async Task<YouTube> StartStreaming(YouTube YouTube)
        {
            YouTube result = new YouTube();
            if (YouTube != null)
            {
                //collect variables from client
                string Message = string.Empty;
                string Api_Key = YouTube.API_KEY;
                string Access_Token = YouTube.Access_Token;

                if (!string.IsNullOrEmpty(Api_Key))
                {
                    string url = $"{YOUTUBE_BASE_URL}?part=snippet%2Ccdn%2CcontentDetails%2Cstatus&key={Api_Key}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject(YouTube);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    
                    //Set headers
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Access_Token);
                    httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    

                    try
                    {
                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<YouTube>(jsonString);

                            result = new YouTube
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode
                            };
                        }
                        else
                        {
                            result = new YouTube
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = $"StartStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new YouTube
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }
                }
                else
                {
                    result = new YouTube
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new YouTube
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }

        [HttpPost]
        [Route("Streaming")]
        public async Task<YouTubeModel> Streaming(YouTube YouTube)
        {
            YouTubeModel result = new YouTubeModel();
            if (YouTube != null)
            {
                //collect variables from client
                string Message = string.Empty;
                string Api_Key = YouTube.API_KEY;
                string Access_Token = YouTube.Access_Token;
                var video = YouTube.File;

                if (!string.IsNullOrEmpty(Api_Key))
                {
                    string url = $"{YOUTUBE_BASE_URL}?part=snippet&key={Api_Key}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject(YouTube);
                    var content = new StringContent(json, Encoding.UTF8, video.ContentType);
                    content.Headers.ContentLength = video.Length;

                    //Set headers
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(video.ContentType));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Access_Token);
                    httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    try
                    {
                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<YouTube>(jsonString);

                            result = new YouTubeModel
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode
                            };
                        }
                        else
                        {
                            result = new YouTubeModel
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = $"StartStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new YouTubeModel
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }

                }
                else
                {
                    result = new YouTubeModel
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new YouTubeModel
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }
    }
}
