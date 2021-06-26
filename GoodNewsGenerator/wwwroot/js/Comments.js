let commentsDisplaySwitcherElement = document.getElementById('GetComments');
let isCommentsDisplayed = false;

function toggleComments(newsId) {
    if (commentsDisplaySwitcherElement != null) {
        if (isCommentsDisplayed == true) {
            commentsDisplaySwitcherElement.innerHTML = 'Display comments';
            document.getElementById('comments-container').innerHTML = '';
        } else {
            commentsDisplaySwitcherElement.innerHTML = 'Hide comments';
            let commentsContainer = document.getElementById('comments-container');
            loadComments(newsId, commentsContainer);

        }
        isCommentsDisplayed = !isCommentsDisplayed;
    }

    commentsDisplaySwitcherElement.addEventListener('onclose', function () {
        alert('closed');
    }, false);
}

function loadComments(newsId, commentsContainer) {
    let request = new XMLHttpRequest();
    //create request
    request.open('GET', `/Comments/List?newsId=${newsId}`, true);
    //create request handler
    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            let resp = request.responseText;
            commentsContainer.innerHTML = resp;

            document.getElementById('create-comment-btn')
                .addEventListener("click", createComment);
        }
    }
    //send request
    request.send();
}

function validateCommentData() {

}

function createComment() {

    let commentText = document.getElementById('commentText').value;
    let newsId = document.getElementById('newsId').value;

    validateCommentData();

    var postRequest = new XMLHttpRequest();
    postRequest.open("POST", '/Comments/Create', true);
    postRequest.setRequestHeader('Content-Type', 'application/json');

    //let requestData = new {
    //    commentText: commentText
    //}

    postRequest.send(JSON.stringify({
        commentText: commentText,
        newsId: newsId
    }));

    postRequest.onload = function () {
        if (postRequest.status >= 200 && postRequest.status < 400) {
            document.getElementById('commentText').value = '';

            //commentsContainer.innerHTML += '';

            loadComments(newsId);
        }
    }
}

var getCommentsIntervalId = setInterval(function () {
    loadComments(newsId);
}, 15000);

//document.onmousemove = function (e) {
//    let mousecoords = getMousePos(e);
//    console.log(`x = ${mousecoords.x} y =${mousecoords.y}`);
//};
//function getMousePos(e) {
//    return { x: e.clientX, y: e.clientY };
//}

//commentsDisplaySwitcherElement.onmouseover = function () {
//    commentsDisplaySwitcherElement.className = commentsDisplaySwitcherElement.className.replace("btn-primary", "btn-info");
//}
//commentsDisplaySwitcherElement.onmouseout = function () {
//    commentsDisplaySwitcherElement.className = commentsDisplaySwitcherElement.className.replace("btn-info", "btn-primary");
//}
/*
 * Mouse events
 * click
 * contextmenu
 * mouseover/mouseout
 * mousedown / mouseup
 * mousemove
 *
 * Form control events
 * submit
 * change
 * focus
 *
 * Keyboard events
 * keydown / keyup
 *
 * Document events
 * DOMContentLoaded
 */