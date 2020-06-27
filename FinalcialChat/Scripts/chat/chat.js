
var messageListContainer;

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
    let html = `
        <div>
            <h2>${message.Content}</h2>
            <div>
                <p>${message.CreatedBy}</p>
                <p>${message.CreatedDate}</p>
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
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const chatRoomId = urlParams.get('chatRoomId')
    if (!chatRoomId) return;
    let data = {
        Content: value,
        ChatroomId: chatRoomId
    };
    post('Chat/AddMessage', data, (newMessage) => {
        addNewMessage(newMessage);
    });
}

function onImputChange(event) {
    let value = event.target.value;
    let button = $("#sendMessage");
    button.prop("disabled", !value)
}

$(document).ready(() => {
    $("#messageInput").on("keyup", onImputChange);
    messageListContainer = document.getElementById('messageList');
    updateScrool();
});