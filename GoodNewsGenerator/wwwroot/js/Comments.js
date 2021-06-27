let commentsDisplaySwitcherElement = document.getElementById('GetComments'); // получаем html блок по id 
let isCommentsDisplayed = false; //создаём флаг и зарание устанавливаем в false

//функция получение и скытия коментариев
function toggleComments(newsId) { 
    if (commentsDisplaySwitcherElement != null) {
        if (isCommentsDisplayed == true) {
            commentsDisplaySwitcherElement.innerHTML = 'Display comments';
            
        } else {
            commentsDisplaySwitcherElement.innerHTML = 'Hide comments';
            let commentsContainer = document.getElementById('comments-container');// получаем весь html со всеми коментариями
            loadComments(newsId, commentsContainer); // передаём все коментарии и id новости в функцию получения коментариев

        }
        isCommentsDisplayed = !isCommentsDisplayed;// флаг отвечающий за отслеживания показаны ли коментарии устанавливаем в истену
    }

    commentsDisplaySwitcherElement.addEventListener('onclose', function () {
        alert('closed');
    }, false);
}


// создаём гет запрос на получения всех коментариев из бд там храняться все коментарии которые уже отображен и те которе заберуться из бд  
function loadComments(newsId, commentsContainer) {
    let request = new XMLHttpRequest();// создаём http запрос
    //create request
    request.open('GET', `/News/Details?id=${newsId}`, true);//говорим что этот запрос типа get  указываем путь
    //create request handler
    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {// если ответ без ошибки и есть контент
            let resp = request.responseText; // получаем ответ в переменную resp
            commentsContainer.innerHTML = resp; //передаём ответ в html блок

            document.getElementById('create-comment-btn')// получаем html по id
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

    var postRequest = new XMLHttpRequest(); // создаём http запрос
    postRequest.open("POST", '/News/AddComment', true); // настраиваем http запрос говоря что он пост указывая одрес
    postRequest.setRequestHeader('Content-Type', 'application/json');//настраиваем пост запроса указывая тип контента запроса json

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