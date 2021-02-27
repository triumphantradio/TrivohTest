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
    public class TwitchController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string TWITCH_BASE_URL = "https://api.twitch.tv/kraken/videos";
        private readonly ILogger<TwitchController> logger;
        public TwitchController(HttpClient _httpClient, ILogger<TwitchController> _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }

        [HttpPost]
        [Route("StartStreaming")]
        public async Task<Twitch> StartStreaming(Twitch twitch)
        {
            Twitch result = new Twitch();
            if (twitch != null)
            {
                //collect variables from client
                string Authorization = twitch.Authorization_Token;
                string Message = string.Empty;
                string Title = twitch.Title;
                string ChannelID = twitch.Channel_Id;
                string ClientID = twitch.Client_Id;

                if(!string.IsNullOrEmpty(Authorization) 
                    && !string.IsNullOrEmpty(Title) 
                    && !string.IsNullOrEmpty(ChannelID) 
                    && !string.IsNullOrEmpty(ClientID))
                {
                    string url = $"{TWITCH_BASE_URL}?channel_id={ChannelID}&title={Title}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject("");
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //Set headers
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.twitchtv.v5+json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", Authorization);
                    httpClient.DefaultRequestHeaders.Add("Client-ID", "uo6dggojyb8d6soh92zknwmi5ej1q2");

                    try
                    {
                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<Twitch>(jsonString);

                            result = new Twitch
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode,
                                Video_Id = model.Video_Id,
                                Upload_Token=model.Upload_Token
                            };
                        }
                        else
                        {
                            result = new Twitch
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch(Exception ex)
                    {
                        Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new Twitch
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }
                }
                else
                {
                    result = new Twitch
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Twitch
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }

        [HttpPost]
        [Route("Streaming")]
        public async Task<Twitch> Streaming(Twitch twitch)
        {
            Twitch result = new Twitch();
            if (twitch != null)
            {
                //collect variables from client
                string Authorization = twitch.Authorization_Token;
                var video = twitch.File;
                string VideoID = twitch.Video_Id;
                string UploadToken = twitch.Upload_Token;
                string Message = string.Empty;

                if (!string.IsNullOrEmpty(Authorization)
                    && !string.IsNullOrEmpty(VideoID)
                    && video != null)
                {
                    string url = $"{TWITCH_BASE_URL}/upload/{VideoID}?part=1&upload_token={UploadToken}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject("");
                    var content = new StringContent(json, Encoding.UTF8, video.ContentType);
                    content.Headers.ContentLength = video.Length;
                    string Name = $"{Guid.NewGuid().ToString()}{video.FileName}";
                    try
                    {
                        var response = await httpClient.PutAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<Twitch>(jsonString);

                            result = new Twitch
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode
                            };
                        }
                        else
                        {
                            result = new Twitch
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch(Exception ex)
                    {
                        Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new Twitch
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }
                    
                }
                else
                {
                    result = new Twitch
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Twitch
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }

        [HttpPost]
        [Route("EndStreaming")]
        public async Task<Twitch> EndStreaming(Twitch twitch)
        {
            Twitch result = new Twitch();
            if (twitch != null)
            {
                //collect variables from client
                string Authorization = twitch.Authorization_Token;
                var video = twitch.File;
                string VideoID = twitch.Video_Id;
                string UploadToken = twitch.Upload_Token;
                string Message = string.Empty;

                if (!string.IsNullOrEmpty(Authorization)
                    && !string.IsNullOrEmpty(VideoID)
                    && video != null)
                {
                    string url = $"{TWITCH_BASE_URL}/upload/{VideoID}/complete?upload_token={UploadToken}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject("");

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    content.Headers.ContentLength = video.Length;

                    try
                    {
                        var response = await httpClient.PutAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            result = new Twitch
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode
                            };
                        }
                        else
                        {
                            result = new Twitch
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch(Exception ex)
                    {
                        Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new Twitch
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }
                }
                else
                {
                    result = new Twitch
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Twitch
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }
    }
}
