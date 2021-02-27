(function (root, factory) {
  if (typeof define === "function" && define.amd) {
    define([], factory(root));
  } else if (typeof exports === "object") {
    module.exports = factory(root);
  } else {
    root.triumphantWebRTC = factory(root);
  }
})(
  typeof global !== "undefined" ? global : this.window || this.global,
  function (root) {
    ("use strict");
    /* globals MediaRecorder */
    let window = root;
    let mediaRecorder;
    let recordedBlobs;
    let isRecording;

    var triumphantWebRTC = {};
    var supports = !!document.querySelector && !!root.addEventListener; // Feature test
    var settings;

    // Default settings
    var defaults = {
      initVideoClass: "video",
      videoRecorderClass: "recorder",
      mimeType: "video/webm;codecs=opus,vp8",
      videoType: "video/mp4",
      blobType: "video/webm",
      fb_access_token: "",
      callbackBefore: function () {},
      callbackAfter: function () {},
    };

    //Private functions

    /**
     * local function to do the actua streaming
     * @param {*} stream
     */
    function handleSuccess(stream) {
      console.log("getUserMedia() got stream:", stream);
      window.stream = stream;

      const gumVideo = document.querySelector(
        `video#${defaults.initVideoClass}`
      );
      gumVideo.srcObject = stream;
    }

    function showMessage(msg) {
      document.getElementById("errorMsg").innerHTML = msg;
    }
    /**
     * This method process continuous stream to facebook
     */
    async function streamingToFacebook(data) {
      const blob = new Blob(recordedBlobs, { type: defaults.videoType });
      //Creates a form data and appends the recorded video and
      var formData = new FormData();
      formData.append(
        "file",
        blob,
        Math.floor(Math.random() * 100800000000) + 1
      );
      formData.append("id", data.id);
      formData.append("secure_stream_url", data.secure_stream_url);
      try {
        const response = await fetch(
          "https://localhost:44366/facebook/streaming",
          {
            method: "POST",
            body: formData,
          }
        );
        const content = await response.json();
        console.log(content);
        return content;
      } catch (err) {
        console.log(err);
      }
    }
    /**
     * Check if there is streamed content to record
     * @param {*} event
     */
    function handleDataAvailable(event) {
      if (event.data && event.data.size > 0) {
        recordedBlobs.push(event.data);
      }
    }

    /**
     * Initializes video streaming
     * @param {*} constraints
     */
    async function initLocal(constraints) {
      try {
        await navigator.mediaDevices
          .getUserMedia(constraints)
          .then((stream) => {
            //(stream) => (video.srcObject = stream);
            handleSuccess(stream);
          });
      } catch (e) {
        console.error("navigator.getUserMedia error:", e);
      }
    }

    // Object for public APIs
    /**
     * Destroy the current initialization of the plugin.
     * @public
     */
    triumphantWebRTC.destroy = function () {
      // If plugin isn't already initialized, stop
      if (!settings) return;

      // Remove init class for conditional CSS
      document.documentElement.classList.remove(settings.videoRecorderClass);

      // Reset variables
      settings = null;
    };

    /**
     * Initialize Plugin. Incase the user needs to manually initialize with custom parameters
     * this case you give params in an object format {a,b...}
     * @public
     * @param {Object} options User settings
     */
    triumphantWebRTC.init = function (options) {
      // feature test
      if (!supports) return;

      // Destroy any existing initializations
      triumphantWebRTC.destroy();

      // Merge user options with defaults
      settings = root.extend(defaults, options || {});

      // Add class to HTML element to activate conditional CSS
      document.documentElement.classList.add(settings.videoRecorderClass);
    };

    /**
     * exposed public function to call when a user wants to start playing
     * the recorded video
     * @param {*} el_id
     */
    triumphantWebRTC.playVideo = function (el_id) {
      showMessage("Your video is playing");
      const superBuffer = new Blob(recordedBlobs, { type: defaults.blobType });
      let recordedVideo = document.getElementById(el_id);
      recordedVideo.src = null;
      recordedVideo.srcObject = null;
      recordedVideo.src = window.URL.createObjectURL(superBuffer);
      recordedVideo.controls = true;
      recordedVideo.play();
    };

    /**
     * downloads the recorded video
     */
    triumphantWebRTC.downloadVideo = function () {
      const blob = new Blob(recordedBlobs, { type: defaults.videoType });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.style.display = "none";
      a.href = url;
      a.download = defaults.videoType;
      document.body.appendChild(a);
      a.click();
      setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      }, 100);
    };

    /**
     * public function to start recording video
     */
    triumphantWebRTC.startRecording = function () {
      showMessage("Your video is recording");
      recordedBlobs = [];
      let options = { mimeType: defaults.mimeType };
      try {
        mediaRecorder = new MediaRecorder(window.stream, options);
      } catch (e) {
        console.error("Exception while creating MediaRecorder:", e);
        return;
      }
      mediaRecorder.onstop = (event) => {
        console.log("Recorder stopped: ", event);
      };

      mediaRecorder.ondataavailable = handleDataAvailable;
      mediaRecorder.start();
    };

    /**
     * exposed plugin function to stop recording video
     */
    triumphantWebRTC.stopRecording = function () {
      showMessage("Recording has been stoped. Please click start");
      mediaRecorder.stop();
    };

    /**
     * exposed plugin function to start video streaming
     */
    triumphantWebRTC.startVideo = async function () {
      showMessage("You are in video mode: Click Record to record this session");
      const constraints = {
        audio: {
          echoCancellation: { exact: false },
        },
        video: {
          width: 1280,
          height: 720,
        },
      };
      await initLocal(constraints);
    };

    /**
     * this method streams live video to facebook
     * @param {*} options
     */
    triumphantWebRTC.uploadToFacebook = async function (options) {
      showMessage("You are uploading to facebook");
      (async () => {
        try {
          let formData = new FormData();
          // Push our data into our FormData object
          if (options) {
            formData.append("access_token", defaults.access_token);
            formData.append("page_id", options.page_id);
            const blob = new Blob(recordedBlobs, { type: defaults.videoType });
            //Creates a form data and appends the recorded video and
            formData.append(
              "file",
              blob,
              Math.floor(Math.random() * 100800000000) + 1
            );

            //Initializes the upload
            const response = await fetch(
              "https://localhost:44366/facebook/startupload",
              {
                method: "POST",
                body: formData,
              }
            );
            const content = await response.json();

            //post the file continue upload
            debugger;
            if (content.isSuccess) {
              formData.append("upload_session_id", content.upload_Session_Id);

              const response2 = await fetch(
                "https://localhost:44366/facebook/uploading",
                {
                  method: "POST",
                  body: formData,
                }
              );
              const content2 = await response2.json();
              //end the continue upload
              if (content2.isSuccess) {
                const response3 = await fetch(
                  "https://localhost:44366/facebook/endupload",
                  {
                    method: "POST",
                    body: formData,
                  }
                );
                const content3 = await response3.json();
                debugger;
                if (content3.isSuccess) {
                  alert("Video uploaded!");
                }
              }
            }
          } else {
            alert(
              "Provide all parameters: access_token, page_id, file to upload!"
            );
          }
          return content;
        } catch (err) {
          console.log(err);
        }
      })();
    };

    /**
     * this method streams live video to facebook
     * @param {*} options
     */
    triumphantWebRTC.startStreamingToFacebook = async function (options) {
      showMessage("You are streaming to facebook");
      (async () => {
        try {
          let formData = new FormData();
          // Push our data into our FormData object
          if (options && options.access_token) {
            formData.append("access_token", options.access_token);
          } else {
            formData.append("access_token", defaults.fb_access_token);
          }

          const response = await fetch(
            "https://localhost:44366/facebook/startstreaming",
            {
              method: "POST",
              body: formData,
            }
          );
          const content = await response.json();
          debugger;
          if (content.isSuccess) {
            const sResponse = streamingToFacebook({
              id: content.id,
              secure_stream_url: content.secure_Stream_Url,
            });
            if (sResponse.isSuccess) {
              setInterval(function () {
                streamingToFacebook({
                  id: sResponse.id,
                  secure_stream_url: sResponse.secure_Stream_Url,
                });
              }, 20000);
              alert("Video streaming started");
            }
          }
          console.log(content);
          return content;
        } catch (err) {
          console.log(err);
        }
      })();
    };

    /**
     * streams live video to youtube
     * @param {*} options
     */
    triumphantWebRTC.streamToYouTube = function (options) {};

    /**
     * streams live video to twitch
     * @param {*} options
     */
    triumphantWebRTC.streamToTwitch = function (options) {};
    /**
     * streams live video to periscope
     * @param {*} options
     */
    triumphantWebRTC.streamToPeriscope = function (options) {};
    return triumphantWebRTC;
  }
);
