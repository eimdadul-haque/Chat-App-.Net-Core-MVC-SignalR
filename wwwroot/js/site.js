
let reciver = "";
let sender = "";
let reciverId = "";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5058/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().catch((err) => log(err))

function CurrentUser(user_name) {
    sender = user_name;
}

connection.on("Connected", (id, name) => {
    var person = document.createElement("div");
    person.innerText = name;
    person.id = id;
    person.onclick = () => {
        reciver = person.innerText;
        reciverId = person.id;
        $.get(`http://localhost:5058/Home/GetMessage?sender=${sender}&reciver=${name}`, (data, status) => {
            data.map((msg, index) => {
                var mes = document.createElement("div");
                mes.innerText = msg.msgBody;

                if (msg.reciver === sender) {
                    mes.className = "message-recive"
                }
                else {
                    mes.className = "message-send"
                }

                document.getElementById("chat").appendChild(mes);
            });

        });
    };
    person.className = "active-person";
    document.getElementById("active-person").appendChild(person); 56
});

connection.on("DisConnected", (id, name) => {
    var per = document.getElementById(id);
    per.removeChild();
});

connection.on("msg_one_user", (msg, name, id) => {
    var mes = document.createElement("div");
    mes.innerText = msg;
    mes.className = "message-recive"
    document.getElementById("chat").appendChild(mes);
    reciverId = id;
    reciver = name;
});

connection.on("msg_back_to_user", (msg, name) => {
    var mes = document.createElement("div");
    mes.innerText = msg;
    mes.className = "message-send"
    document.getElementById("chat").appendChild(mes);
});

function send(name) {
    var message = document.getElementById("msg").value;
    $.post("http://localhost:5058/Home/SaveMessage", {
        msgBody: message,
        sender: name,
        reciver: reciver
    }, (data, status) => {
        if (status === "success") {
            connection.invoke("SendMessageToOneUser", message, name, reciverId).catch(err => console.log(err));
            connection.invoke("BackToSender", message, name).catch(err => console.log(err));
            document.getElementById("msg").value = "";
        }
    });
}


