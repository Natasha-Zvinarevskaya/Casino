// script.js — websocket + UI for BlackJack
// Важно: DTO-формат сообщений — { Controller: "BlackJackGame", Method: "...", Value: { ... } }

// --- WebSocket connection (adapted from your uploaded scripts.js) ---
function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) {
    return parts.pop().split(';').shift();
  }
  return null;
}

let socket = null;
let currentGame = null;

function createWebSocket() {
  // если нужен токен — используется cookie "Auth-Token" (как в примере)
  const token = getCookie("Auth-Token");
  // Если токена нет — всё равно можно попытаться подключиться к ws на том же хосте (без токена)
  const protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
  const host = window.location.host;
  let url = `${protocol}//${host}/ws`;
  if (token) url += `?token=${encodeURIComponent(token)}`;

  console.log("Connecting to WS:", url);
  const s = new WebSocket(url);

  s.onopen = () => {
    setConnState("connected");
    console.log("WS open");
  };

  s.onmessage = (ev) => {
    try {
      const data = JSON.parse(ev.data);
      console.log("WS message", data);
      // Ожидаем что сервер шлёт BaseResponse { IsSucces, ErrorMessage, Data }
      handleBaseResponse(data);
    } catch (e) {
      console.warn("Failed parse ws message:", ev.data, e);
    }
  };

  s.onclose = () => {
    setConnState("closed");
    console.log("WS closed");
  };

  s.onerror = (err) => {
    setConnState("error");
    console.error("WS err", err);
  };

  return s;
}

function setConnState(text) {
  const el = document.getElementById("connState");
  if (!el) return;
  el.textContent = text;
  if (text === "connected") el.style.color = "var(--accent)";
  else if (text === "closed" || text === "error") el.style.color = "#ff8b8b";
  else el.style.color = "";
}

// start
socket = createWebSocket();
if (socket === null) setConnState("no-socket");

// --- Helpers ---
function sendMessage(controller, method, value) {
  if (!socket || socket.readyState !== WebSocket.OPEN) {
    showToast("Нет соединения с сервером", true);
    return;
  }
  const msg = { Controller: controller, Method: method, Value: value };
  socket.send(JSON.stringify(msg));
  console.log("Send:", msg);
}

function showToast(text, isError = false, ms = 3000) {
  const t = document.getElementById("toast");
  t.textContent = text;
  t.className = "toast " + (isError ? "error" : "success");
  t.style.display = "block";
  clearTimeout(t._to);
  t._to = setTimeout(() => (t.style.display = "none"), ms);
}

function showModal(title, subtitle) {
  const modal = document.getElementById("modal");
  document.getElementById("modalMessage").textContent = title;
  document.getElementById("modalSub").textContent = subtitle || "";
  modal.style.display = "flex";
}

function hideModal() {
  const modal = document.getElementById("modal");
  modal.style.display = "none";
}

// --- Render cards ---
function renderCard(card) {
    if (!card) return null;

    const suitSymbols = ["♥", "♦", "♣", "♠"];
    const suit = suitSymbols[card.Suit] ?? "?";

    const valueMap = {
        2: "2", 3: "3", 4: "4", 5: "5",
        6: "6", 7: "7", 8: "8", 9: "9",
        10: "10", 11: "A"
    };
    const val = valueMap[card.Value] ?? String(card.Value);

    const div = document.createElement("div");
    div.className = "cardItem animate-card " + (suit === "♥" || suit === "♦" ? "card red" : "card black");
    div.innerHTML = `
    <div class="cardTop">${suit}</div>
    <div class="cardCenter">${val}</div>
    <div class="cardBottom">${suit}</div>
  `;

    return div;
}

function renderGameModel(model) {
    currentGame = model;
    document.getElementById("currentGameId").textContent = model.GameId ?? "—";

    const dealer = document.getElementById("dealerCards");
    const player = document.getElementById("playerCards");
    dealer.innerHTML = "";
    player.innerHTML = "";

    (model.DealerCards || []).forEach(c => {
        const el = renderCard(c);
        dealer.appendChild(el);
    });

    (model.PLayerCards || []).forEach(c => {
        const el = renderCard(c);
        player.appendChild(el);
    });

    // скрыть ставки если есть активная игра
    if (model.Status === 0) {
        document.getElementById("betSection").style.display = "none";
    }

    handleStatus(model.Status);
}

function handleStatus(statusRaw) {
    const status = Number(statusRaw); // всегда число

    const hitBtn = document.getElementById("hitBtn");
    const standBtn = document.getElementById("standBtn");
    const newBtn = document.getElementById("newGameBtn");

    hitBtn.style.display = "none";
    standBtn.style.display = "none";
    newBtn.style.display = "none";

    if (status === 0) { // None
        hitBtn.style.display = "inline-flex";
        standBtn.style.display = "inline-flex";
    } else if (status === 1) { // Win
        showModal("Вы победили! 🎉", "Поздравляю — выигрыш зачислен.");
        newBtn.style.display = "inline-flex";
        document.getElementById("betSection").style.display = "block";
    } else if (status === 2) { // Loss
        showModal("Вы проиграли.", "Не повезло — можно попробовать снова.");
        newBtn.style.display = "inline-flex";
        document.getElementById("betSection").style.display = "block";
    } else if (status === 3) { // Draw
        showModal("Ничья.", "Ставка возвращена.");
        newBtn.style.display = "inline-flex";
        document.getElementById("betSection").style.display = "block";
    }
}

// --- Handle server BaseResponse ---
function handleBaseResponse(resp) {
  // Expect object like { IsSucces: bool, ErrorMessage: string, Data: BlackJackGameModel }
  // Field name in spec is IsSucces (без второго s) — используем именно её.
  if (!resp) return;
  const ok = resp.IsSucces === true;
  if (!ok) {
    const msg = resp.ErrorMessage || "Ошибка от сервера";
    showToast(msg, true);
    return;
  }
  const model = resp.Data;
  if (!model) {
    showToast("Пустая модель от сервера", true);
    return;
  }
  renderGameModel(model);
}

// --- UI wiring ---
document.addEventListener("DOMContentLoaded", () => {
  const startBtn = document.getElementById("startBtn");
  const betSelect = document.getElementById("betSelect");
  const hitBtn = document.getElementById("hitBtn");
  const standBtn = document.getElementById("standBtn");
  const newBtn = document.getElementById("newGameBtn");
  const modalRestart = document.getElementById("modalRestart");

  startBtn.addEventListener("click", () => {
    const bet = Number(betSelect.value);
    startBtn.classList.add("loading");
    startBtn.disabled = true;
    // Запрос: Controller "BlackJackGame", Method "StartGame", Value: { Bet: <number> }
    sendMessage("BlackJackGame", "StartGame", { Bet: bet });
    // Убираем индикацию через некоторое время (в случае, если ответ долго)
    setTimeout(() => {
      startBtn.classList.remove("loading");
      startBtn.disabled = false;
    }, 1200);
  });

  hitBtn.addEventListener("click", () => {
    if (!currentGame || !currentGame.GameId) {
      showToast("Нет активной игры", true);
      return;
    }
    // TurnPlayer expects Value: { GameId }
    sendMessage("BlackJackGame", "TurnPlayer", { GameId: currentGame.GameId });
  });

  // "Пропустить" — метод на сервере не указан в ТЗ, делаю best-effort: вызываю "SkipPlayer"
  // Если на сервере другой метод — можно изменить на "Stand" или "SkipTurn"
  standBtn.addEventListener("click", () => {
    if (!currentGame || !currentGame.GameId) {
      showToast("Нет активной игры", true);
      return;
    }
    // Мы будем вызывать метод "SkipPlayer"
    sendMessage("BlackJackGame", "SkipPlayer", { GameId: currentGame.GameId });
  });

  newBtn.addEventListener("click", () => {
    hideModal();
    // Сбрасываем UI, оставляем возможность выбрать новую ставку
    document.getElementById("dealerCards").innerHTML = "";
    document.getElementById("playerCards").innerHTML = "";
    document.getElementById("currentGameId").textContent = "—";
    currentGame = null;
    showToast("Готово к новой игре");
  });

  modalRestart.addEventListener("click", () => {
    hideModal();
    // emulate new game button click
    document.getElementById("newGameBtn").click();
  });
});
