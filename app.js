// setup signalR
const clientId = Math.floor(Math.random() * 1000000000000) + '';
const apiBaseUrl = "https://qinezh-func.azurewebsites.net";

const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${apiBaseUrl}/api`)
    .build();
connection.on("contentUpdated", contentUpdated);
connection.start()
    .then(() => Console.log("connected"))
    .catch(err => console.err);

// setup editor
const editor = monaco.editor.create(document.getElementById("container"), {
    value: "",
    language: "csharp"
});

const container = document.getElementById("container");

let hasUnsentEvent = false;
container.addEventListener("keyup", () => {
    hasUnsentEvent = true;
});

function contentUpdated(data) {
    if (data.sender === clientId) {
        return;
    }

    editor.setValue(data.content);
}

setInterval(() => {
    if (hasUnsentEvent) {
        axios.post(`${apiBaseUrl}/api/update`, {
            sender: clientId,
            content: editor.getValue()
        });
        hasUnsentEvent = false;
    }
}, 500);
