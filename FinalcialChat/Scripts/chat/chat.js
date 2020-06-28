
var messageListContainer, chat, chatRoomId;

const get = (path, callback) => {
    fetch(path)
        .then(x => x.json())
        .then(response => {
            callback && callback(response)
        }).catch(error => {
            console.log(error);
        })
}

const post = (path, data, callback) => {
    fetch(path, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
        .then(x => x.json())
        .then(data => {
            callback && callback(data)
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function addRoom() {
    let tBody = document.querySelector("#roomTable tbody");
    tBody.innerHTML = '';

    $('#roomsModal').modal({});

    get('Chat/GetRooms', (rooms) => {
        let html = '';
        rooms.forEach(room => {
            html += `
                <tr>
                  <td>${room.Name}</td>
                  <td>
                    <input class="room-id" type="checkbox" id="room-${room.Id}" name="room-${room.Id}" value="${room.Id}">
                  </td>
                </tr>
            `;
        });
        let tBody = document.querySelector("#roomTable tbody");
        tBody.innerHTML = html;
    });
}

function saveRoom() {
    let checkboxes = document.getElementsByClassName('room-id')
    var roomIds = [];
    Array.prototype.forEach.call(checkboxes, function (el) {
        if (el.checked) {
            roomIds.push(el.value);
        }
    });

    post('Chat/AddRooms', roomIds, () => {
        location.reload();
    });
}

function addNewMessage(message) {
    let date = new Date(message.CreatedDate);
    let formatDate = `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()}
        ${date.getHours()}:${date.getMinutes()}`;

    let html = `
        <div class="messages-container">
            <h2>${message.Content}</h2>
            <div class="message-info">
                <p>${message.CreatedBy}, ${formatDate}</p>
            </div>
        </div>
    `;
    messageListContainer.innerHTML += html;
    updateScrool();
}

function updateScrool() {
    messageListContainer.scrollTo(0, messageListContainer.scrollHeight + 1000);
}

function sendMessage() {

    let value = $("#messageInput").val();
    if (!value) return;
    if (!chatRoomId) return;
    let data = {
        Content: value,
        ChatroomId: chatRoomId
    };

    chat.server.send(data);
    $("#messageInput").focus();
    $("#sendMessage").prop("disabled", true);
}

function onImputChange(event) {
    console.log(event.target.id);
    if (event.target.id !== 'messageInput') return;

    let value = event.target.value;
    let button = $("#sendMessage");
    button.prop("disabled", !value || !chatRoomId)
    if (event.keyCode === 13) {
        sendMessage();
    }
}

function startConnection() {
    chat = $.connection.messageHub;
    chat.client.addChatMessage = function (newMessage) {
        addNewMessage(newMessage);
        $("#messageInput").focus();
    };
    $.connection.hub.start().done(function (response) {
        console.log('connected with id: ' + response.id)
    });
}

$(document).ready(() => {
    $("body").on("keyup", onImputChange);
    messageListContainer = document.getElementById('messageList');

    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    chatRoomId = urlParams.get('chatRoomId')

    if (!chatRoomId) {
        let rooms = $('.room-name a');
        rooms && rooms.length > 0 && rooms[0].click();
    }
    
    updateScrool();
    startConnection();

    $("#messageInput").focus();
});
