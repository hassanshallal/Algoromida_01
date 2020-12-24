// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code

//Speech recognition on chrome
function runSpeechRecognition() {
    
    var output = document.getElementById("output");

    var action = document.getElementById("action");
    if ('SpeechRecognition' in window || 'webkitSpeechRecognition' in window) {
        var SpeechRecognition = SpeechRecognition || webkitSpeechRecognition;
        var recognition = new SpeechRecognition();

        recognition.onstart = function () {
            action.innerHTML = "<small>listening, please speak...</small>";
        };

        recognition.onspeechend = function () {
            action.innerHTML = "<small>stopped listening, hope you are done...</small>";
            recognition.stop();
        }

        recognition.onresult = function (event) {
            var transcript = event.results[0][0].transcript;
            var confidence = event.results[0][0].confidence;
            output.innerHTML = transcript;
            output.classList.remove("hide");
        };
        recognition.start();
    } else {
        output.innerHTML = "Algoromida bots can hear you only in Google Chrome, please type your message and click submit to send it.";
    }
};


// Make sure last interactions show on the page

$('#chatmessages').scrollTop($('#chatmessages')[0].scrollHeight);