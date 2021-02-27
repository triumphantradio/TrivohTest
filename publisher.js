import * as FB from 'https://connect.facebook.net/es_LA/sdk.js';

let streamToFacebook = function(options){
    FB.api(
        `/${options.page_id}/live_videos?status=LIVE_NOW&title=${options.title}&description=${options.description}&access_token=${access_token}`,
        'POST',
        function(response) {
            // Insert your code here
        }
      );
}


let streamToTwith = function(){
    
}


let streamToYoutube = function(){
    
}

let streamToPeriscope = function(){
    
}