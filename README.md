FOR A LIVE DEMO

visit: https://jovial-swanson-32b018.netlify.app/

TO TEST
NB: there are 2 branches, vpr and vpr-backend-api, one is the front end to record, play or download the video the vpr-backend-api processes the the publishing to social media

1. Launch the index page
2. Click on "Start Camera"
3. Click on "Record" button to start recording.
4. Click on "Stop" button to stop record. Do this before you play or download
5. Click on "Play" button to play the recorded video
6. Click on "Download" button to download the recorded video

TO USE OF CONNECT TO ANOTHER PROJECT

1. all functions are exposed through "triumphantWebRTC"
E.G:
1. triumphantWebRTC.init({initVideoClass: yourclassname, videoRecorderClass: classtodisplayrecordedvideo }) to initialize the plugin
2. triumphantWebRTC.destroy(); to destroy the initialized instance
3. triumphantWebRTC.playVideo(element_id);
4. triumphantWebRTC.downloadVideo(); // to download the video
5. triumphantWebRTC.stopRecording(); // stop recording
6. triumphantWebRTC.startVideo(); // to start video
7. triumphantWebRTC.uploadToFacebook({access_token: YOUR_ACCESS_TOKEN, page_id: YOUR_PAGE_ID}) //to upload to facebook
8. triumphantWebRTC.startStreamingToFacebook({access_token: YOUR_ACCESS_TOKEN, page_id: YOUR_PAGE_ID});


The .NET project is the server side api that streams the recorded video to facebook, youtube and twitch.
with the endpoints

https://domain.com/facebook/startstreaming
https://domain.com/facebook/streaming
https://domain.com/facebook/endstreaming



https://domain.com/youtube/startstreaming
https://domain.com/youtube/streaming
https://domain.com/youtube/endstreaming



https://domain.com/twitch/startstreaming
https://domain.com/twitch/streaming
https://domain.com/twitch/endstreaming
