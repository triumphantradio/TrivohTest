// Call back function for google signIn
async function onSignIn(googleUser) {
    try {
        // user Id token for verification on the server
        const idToken = googleUser.getAuthResponse().id_token
        if (idToken) {
            // Authenticating user before proceeding to stream
            const done = await googleAPI(idToken)
            if (done) {
                alert('Sign In successful')
            }
        }
    } catch (e) {
        console.log("Error", e)
        alert('An error occurred. Try again')
    }
}

async function googleAPI(idToken) {
    // network call to authenticate user google ID Token
    return axios({
        method: 'post',
        url: 'https://video-recorder-delta.vercel.app/',
        data: {
            idToken
        }
    })
}


