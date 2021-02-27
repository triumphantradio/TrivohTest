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
    public class FacebookController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly string FB_BASE_URL = "https://graph.facebook.com/v10.0";
        private readonly string FB_UPLOAD_URL = "https://graph.facebook.com/v10.0";
        private readonly ILogger<FacebookController> logger;
        public FacebookController(HttpClient _httpClient, ILogger<FacebookController> _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }

        [HttpPost]
        [Route("startupload")]
        public async Task<Facebook> StartUpload([FromForm] Facebook Facebook)
        {
            Facebook result = new Facebook();
            if (Facebook != null)
            {
                //collect variables from client
                string Message = string.Empty;
                string AccessToken = Facebook.Access_Token;

                if (!string.IsNullOrEmpty(AccessToken))
                {
                    string url = $"{FB_UPLOAD_URL}/{Facebook.Page_Id}/videos?upload_phase=start&access_token={AccessToken}&file_size={Facebook.File.Length}";

                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject("");
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<Facebook>(jsonString);

                            result = new Facebook
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode,
                                Id = model.Id,
                                Upload_Session_Id = model.Upload_Session_Id
                            };
                        }
                        else
                        {
                            result = new Facebook
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new Facebook
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }
                }
                else
                {
                    result = new Facebook
                    {
                        Message = "Please provide: Access Token.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Facebook
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }

        [HttpPost]
        [Route("uploading")]
        public async Task<Facebook> Uploading([FromForm] Facebook Facebook)
        {
            Facebook result = new Facebook();
            if (Facebook != null)
            {
                //collect variables from client
                var video = Facebook.File;
                string Message = string.Empty;

                if (video != null && !string.IsNullOrEmpty(Facebook.Upload_Session_Id) && Facebook.Upload_Session_Id != "undefined")
                {
                    //Sets false content just to be able to use httpClient.PostAsync()
                    var json = JsonConvert.SerializeObject(Request.Body);
                    var content = new StringContent(json, Encoding.UTF8, Request.ContentType);
                    content.Headers.ContentLength = Request.Headers.ContentLength;
                    

                    try
                    {
                        string url = $"{FB_UPLOAD_URL}/{Facebook.Page_Id}/videos?upload_phase=transfer&upload_session_id={Facebook.Upload_Session_Id}&access_token={Facebook.Access_Token}&start_offset=0&video_file_chunk={Facebook.File.FileName}.mp4";
                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var model = JsonConvert.DeserializeObject<Facebook>(jsonString);

                            result = new Facebook
                            {
                                Message = response.RequestMessage.ToString(),
                                IsSuccess = response.IsSuccessStatusCode
                            };
                        }
                        else
                        {
                            result = new Facebook
                            {
                                Message = "Unable to upload video. Please refresh and resubmit",
                                IsSuccess = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                        logger.LogInformation(Message);

                        result = new Facebook
                        {
                            Message = ex.Message,
                            IsSuccess = false
                        };
                    }

                }
                else
                {
                    result = new Facebook
                    {
                        Message = "Please provide: Authorization Token, Channel_ID, Client_ID and Video_Title.",
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Facebook
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }

        [HttpPost]
        [Route("endupload")]
        public async Task<Facebook> EndUpload([FromForm] Facebook Facebook)
        {
            Facebook result = new Facebook();
            string Message = string.Empty;
            string AccessToken = Facebook.Access_Token;

            if (!string.IsNullOrEmpty(AccessToken))
            {
                string url = $"{FB_UPLOAD_URL}/{Facebook.Page_Id}/videos?upload_phase=finish&access_token={Facebook.Access_Token}&upload_session_id={Facebook.Upload_Session_Id}";

                //Sets false content just to be able to use httpClient.PostAsync()
                var json = JsonConvert.SerializeObject("");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        result = new Facebook
                        {
                            Message = response.RequestMessage.ToString(),
                            IsSuccess = response.IsSuccessStatusCode
                        };
                    }
                    else
                    {
                        result = new Facebook
                        {
                            Message = "Unable to upload video. Please refresh and resubmit",
                            IsSuccess = false
                        };
                    }
                }
                catch (Exception ex)
                {
                    Message = $"EndStream endpoint visited at {DateTime.UtcNow.ToLongTimeString()}<br/> Error: {ex.Message} <br/> Stack Trace: {ex.StackTrace}";
                    logger.LogInformation(Message);

                    result = new Facebook
                    {
                        Message = ex.Message,
                        IsSuccess = false
                    };
                }
            }
            else
            {
                result = new Facebook
                {
                    Message = "Provide a correct URL and make sure the video is recorded.",
                    IsSuccess = false
                };
            }
            return result;
        }
    }
}
