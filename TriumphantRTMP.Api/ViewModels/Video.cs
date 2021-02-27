using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TriumphantRTMP.Api.ViewModels
{
    public class Twitch
    {
        [JsonProperty(PropertyName = "authorization_token")]
        public string Authorization_Token { get; set; }
        [JsonProperty(PropertyName = "file")]
        public IFormFile File { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "channel_id")]
        public string Channel_Id { get; set; }
        [JsonProperty(PropertyName = "upload_token")]
        public string Upload_Token { get; set; }
        [JsonProperty(PropertyName = "video_id")]
        public string Video_Id { get; set; }
        [JsonProperty(PropertyName = "client_id")]
        public string Client_Id { get; set; }
        [JsonProperty(PropertyName = "issuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
    public class Facebook
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "file")]
        public IFormFile File { get; set; }
        [JsonProperty(PropertyName = "secure_stream_url")]
        public string Secure_Stream_Url { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string Access_Token { get; set; }
        
        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "start_offset")]
        public string Start_Offset { get; set; }
        [JsonProperty(PropertyName = "upload_session_id")]
        public string Upload_Session_Id { get; set; }
        [JsonProperty(PropertyName = "page_id")]
        public string Page_Id { get; set; }
        [JsonProperty(PropertyName = "issuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
    public class YouTube
    {
        [JsonProperty(PropertyName = "authorization_token")]
        public string Authorization_Token { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "file")]
        public IFormFile File { get; set; }
        [JsonProperty(PropertyName = "api_key")]
        public string API_KEY { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string Access_Token { get; set; }
        [JsonProperty(PropertyName = "issuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "snippet")]
        public Snippet Snippet { get; set; }
    }


    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

    public class YouTubeModel
    {
        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "snippet")]
        public Snippet Snippet { get; set; }

        [JsonProperty(PropertyName = "cdn")]
        public Cdn Cdn { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Status Status { get; set; }

        [JsonProperty(PropertyName = "contentDetails")]
        public ContentDetails ContentDetails { get; set; }
        [JsonProperty(PropertyName = "issuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
    public class Snippet
    {
        [JsonProperty(PropertyName = "publishedAt")]
        public string PublishedAt { get; set; }

        [JsonProperty(PropertyName = "channelId")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isDefaultStream")]
        public string IsDefaultStream { get; set; }
    }

    public class IngestionInfo
    {
        [JsonProperty(PropertyName = "streamName")]
        public string StreamName { get; set; }

        [JsonProperty(PropertyName = "ingestionAddress")]
        public string IngestionAddress { get; set; }

        [JsonProperty(PropertyName = "backupIngestionAddress")]
        public string BackupIngestionAddress { get; set; }
    }

    public class Cdn
    {
        [JsonProperty(PropertyName = "ingestionType")]
        public string IngestionType { get; set; }

        [JsonProperty(PropertyName = "ingestionInfo")]
        public IngestionInfo IngestionInfo { get; set; }

        [JsonProperty(PropertyName = "resolution")]
        public string Resolution { get; set; }

        [JsonProperty(PropertyName = "frameRate")]
        public string FrameRate { get; set; }
    }

    public class ConfigurationIssue
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "severity")]
        public string Severity { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }

    public class HealthStatus
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "lastUpdateTimeSeconds")]
        public string LastUpdateTimeSeconds { get; set; }

        [JsonProperty(PropertyName = "configurationIssues")]
        public List<ConfigurationIssue> ConfigurationIssues { get; set; }
    }

    public class Status
    {
        [JsonProperty(PropertyName = "streamStatus")]
        public string StreamStatus { get; set; }

        [JsonProperty(PropertyName = "healthStatus")]
        public HealthStatus HealthStatus { get; set; }
    }

    public class ContentDetails
    {
        [JsonProperty(PropertyName = "closedCaptionsIngestionUrl")]
        public string ClosedCaptionsIngestionUrl { get; set; }

        [JsonProperty(PropertyName = "isReusable")]
        public string IsReusable { get; set; }
    }

    


}
