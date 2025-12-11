// script.js — MULTIPLAYER BLACKJACK WITH REAL-TIME EVENTS

function getCookie(name) {
    const v = `; ${document.cookie}`;
    const parts = v.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(";")[0];
    return null;
}

let socket = null;
let currentGame = null;
let lastRenderedModel = null;

// ---------------------------
// UI HELPERS
// ---------------------------

function setConnState(text) {
    const el = document.getElementById("connState");
    if (!el) return;
    el.textContent = text;
    el.style.color =
        text === "connected" ? "var(--accent)" :
            text === "error" || text === "closed" ? "#ff6c6c" :
                "#fff";
}

function showToast(text, error = false) {
    const t = document.getElementById("toast");
    if (!t) return alert(text);

    t.textContent = text;
    t.className = "toast " + (error ? "error" : "success");
    t.style.display = "block";

    clearTimeout(t._timer);
    t._timer = setTimeout(() => t.style.display = "none", 3000);
}

function addLog(text, color = "white") {
    const box = document.getElementById("gameLog");
    if (!box) return;

    const div = document.createElement("div");
    div.className = "log-entry";
    div.style.color = color;
    div.textContent = text;

    box.appendChild(div);
    box.scrollTop = box.scrollHeight;
}

function showModal(title, sub) {
    const m = document.getElementById("modal");
    if (!m) return;

    document.getElementById("modalMessage").textContent = title;
    document.getElementById("modalSub").textContent = sub || "";
    m.style.display = "flex";
}

function hideModal() {
    const m = document.getElementById("modal");
    if (!m) return;
    m.style.display = "none";
}

// ---------------------------
// CARD RENDER
// ---------------------------

function renderCard(card) {
    if (!card) return null;

    const suits = ["♥", "♦", "♣", "♠"];
    const suit = suits[card.Suit] ?? "?";

    const valMap = {
        2: "2", 3: "3", 4: "4", 5: "5",
        6: "6", 7: "7", 8: "8", 9: "9",
        10: "10", 11: "A"
    };
    const val = valMap[card.Value] ?? card.Value;

    const d = document.createElement("div");
    d.className = "cardItem card " + (suit === "♥" || suit === "♦" ? "red" : "black");
    d.style.position = "relative";

    d.innerHTML = `
        <div class="cardTop">${suit}</div>
        <div class="cardCenter">${val}</div>
        <div class="cardBottom">${suit}</div>
    `;
    return d;
}

// ---------------------------
// GAME RENDERING
// ---------------------------

function renderGame(model) {
    if (!model) return;
    lastRenderedModel = model;
    currentGame = model;

    const idEl = document.getElementById("currentGameId");
    if (idEl) idEl.textContent = model.GameId ?? "—";

    // Render dealer
    const dealerArea = document.getElementById("dealerCards");
    if (dealerArea) dealerArea.innerHTML = "";

    const dealer = (model.PLayerCards || []).find(p => p.IsDealer);
    if (dealer && dealer.Cards) {
        dealer.Cards.forEach(c => {
            const card = renderCard(c);
            if (card) dealerArea.appendChild(card);
        });
    }

    // Render all players
    const playersArea = document.getElementById("playerCards");
    if (playersArea) playersArea.innerHTML = "";

    (model.PLayerCards || []).forEach(p => {
        if (p.IsDealer) return;

        const wrap = document.createElement("div");
        wrap.className = "card";
        wrap.style.padding = "10px";
        wrap.style.margin = "10px";
        wrap.style.width = "210px";

        const title = document.createElement("div");
        title.innerHTML = `<strong>Игрок ${p.UserId ?? "?"}</strong>`;
        wrap.appendChild(title);

        const score = document.createElement("div");
        score.textContent = "Очки: " + p.Score;
        wrap.appendChild(score);

        const row = document.createElement("div");
        row.style.display = "flex";
        row.style.flexWrap = "wrap";
        row.style.gap = "6px";

        (p.Cards || []).forEach(c => {
            const card = renderCard(c);
            if (card) row.appendChild(card);
        });

        wrap.appendChild(row);
        playersArea.appendChild(wrap);
    });

    // Show action buttons only for active player
    updateButtons(model);
}

function updateButtons(model) {
    const hit = document.getElementById("hitBtn");
    const stand = document.getElementById("standBtn");
    const newBtn = document.getElementById("newGameBtn");

    hit.style.display = "none";
    stand.style.display = "none";
    newBtn.style.display = "none";

    const myId = Number(getCookie("UserId"));
    const me = (model.PLayerCards || []).find(p => p.UserId === myId);

    if (me && me.StatusGame === 0) {
        hit.style.display = "inline-flex";
        stand.style.display = "inline-flex";
    }

    if (!me && model.Status !== 0) {
        newBtn.style.display = "inline-flex";
    }

    if (model.Status === 1) {
        showModal("Вы победили! 🎉");
        newBtn.style.display = "inline-flex";
    }
    if (model.Status === 2) {
        showModal("Вы проиграли!");
        newBtn.style.display = "inline-flex";
    }
    if (model.Status === 3) {
        showModal("Ничья!");
        newBtn.style.display = "inline-flex";
    }
}

// ---------------------------
// WEBSOCKET SETUP
// ---------------------------

function createWs() {
    const token = getCookie("Auth-Token");
    const p = location.protocol === "https:" ? "wss:" : "ws:";
    let url = `${p}//${location.host}/ws`;
    if (token) url += `?token=${token}`;

    let ws = new WebSocket(url);

    ws.onopen = () => setConnState("connected");
    ws.onclose = () => setConnState("closed");
    ws.onerror = () => setConnState("error");

    ws.onmessage = e => {
        try {
            const msg = JSON.parse(e.data);
            handleIncoming(msg);
        } catch (err) {
            console.error("WS parse error:", err);
        }
    };

    return ws;
}

socket = createWs();

// ---------------------------
// SEND MESSAGE
// ---------------------------

function sendMessage(controller, method, value) {
    if (!socket || socket.readyState !== WebSocket.OPEN) {
        showToast("Нет соединения", true);
        return;
    }
    socket.send(JSON.stringify({ Controller: controller, Method: method, Value: value }));
}

// ---------------------------
// INCOMING EVENTS HANDLER
// ---------------------------

function handleIncoming(msg) {
    if (!msg) return;

    const ctrl = msg.Controller;
    const method = msg.Method;

    // 🔵 1) Игрок подключился
    if (ctrl === "GameWsController" && method === "ConnectAnotherPlayer") {
        addLog(`Игрок ${msg.Value?.UserId} подключился`, "#37ff9b");
        return;
    }

    // 🔴 2) Игрок отключился
    if (ctrl === "GameWsController" && method === "DisconnectAnotherPlayer") {
        addLog(`Игрок ${msg.Value?.UserId} отключился`, "#ff6c6c");
        return;
    }

    // 🟡 3) Другой игрок берёт карту
    if (ctrl === "BlackJackGameWsController" && method === "TurnAnotherPlayer") {
        addLog(`Игрок ${msg.Value?.UserId} взял карту`, "#ffe16c");
        return;
    }

    // 🟠 4) Другой игрок пропустил ход
    if (ctrl === "BlackJackGameWsController" && method === "SkipAnotherPlayer") {
        addLog(`Игрок ${msg.Value?.UserId} пропустил ход`, "#dfe7ff");
        return;
    }

    // 🟢 5) Обычный ответ на любые игровые запросы
    if (msg.IsSucces && msg.Data) {
        renderGame(msg.Data);
    }
}

// ---------------------------
// DOM EVENTS
// ---------------------------

document.addEventListener("DOMContentLoaded", () => {
    const startBtn = document.getElementById("startBtn");
    const connectBtn = document.getElementById("connectBtn");
    const disconnectBtn = document.getElementById("disconnectBtn");
    const hitBtn = document.getElementById("hitBtn");
    const standBtn = document.getElementById("standBtn");
    const newBtn = document.getElementById("newGameBtn");
    const betSelect = document.getElementById("betSelect");
    const maxPlayers = document.getElementById("maxPlayers");

    // CREATE GAME
    startBtn.onclick = () => {
        sendMessage("GameWsController", "CreateGame", {
            Game: 0,
            Bet: Number(betSelect.value),
            MaxCountPlayers: Number(maxPlayers.value)
        });
    };

    // CONNECT
    connectBtn.onclick = () => {
        const gid = Number(document.getElementById("currentGameId").textContent);
        if (!gid) return showToast("Нет ID игры", true);

        sendMessage("GameWsController", "ConnectPlayer", { gameId: gid });
    };

    // DISCONNECT
    disconnectBtn.onclick = () => {
        const gid = Number(document.getElementById("currentGameId").textContent);
        sendMessage("GameWsController", "DisconnectPlayer", { gameId: gid });
    };

    // TURN
    hitBtn.onclick = () => {
        sendMessage("BlackJackGameWsController", "TurnPlayer", {
            gameId: currentGame?.GameId
        });
    };

    // SKIP
    standBtn.onclick = () => {
        sendMessage("BlackJackGameWsController", "SkipPlayer", {
            gameId: currentGame?.GameId
        });
    };

    newBtn.onclick = () => location.reload();
});
