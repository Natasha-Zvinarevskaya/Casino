/**
 * Простая фронтенд-авторизация/регистрация для контроллера Auth
 * Маршруты: POST /Auth/Register и POST /Auth/LogIn
 * На сервер уходит только email и password (без повтора пароля).
 * Локальный мок включен по умолчанию (USE_MOCK = true) — удобно тестировать без бэкенда.
 */

// === Настройки ===
const BASE_URL = "http://localhost:5179"; // Пример для прод: "https://api.example.com"
const USE_MOCK = false; // false — реальные запросы, true — ответ эмулируется локально

// === Утилиты ===
const $ = (sel, ctx = document) => ctx.querySelector(sel);
const $$ = (sel, ctx = document) => Array.from(ctx.querySelectorAll(sel));

function setButtonLoading(btn, isLoading) {
  btn.classList.toggle('loading', isLoading);
  btn.disabled = isLoading;
}

function showToast(message, type = 'success', timeout = 3500) {
  const tpl = $('#toastTemplate');
  const node = tpl.content.firstElementChild.cloneNode(true);
  node.textContent = message;
  node.classList.add(type);
  document.body.appendChild(node);
  setTimeout(() => node.remove(), timeout);
}

function setFieldError(input, msg = '') {
  const name = input.getAttribute('id');
  const errorEl = document.querySelector(`.error[data-for="${name}"]`);
  if (errorEl) errorEl.textContent = msg;
  input.setAttribute('aria-invalid', msg ? 'true' : 'false');
}

function validateEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function apiPost(path, payload) {
  if (USE_MOCK) {
    // Эмуляция ответа сервера
    return new Promise((resolve) => {
      setTimeout(() => {
        const ok = true;
        resolve({
          ok,
          status: ok ? 200 : 400,
          json: async () => ok
            ? { success: true, token: 'demo-token', user: { email: payload.email } }
            : { success: false, message: 'Ошибка' },
        });
      }, 700);
    });
  }
  return fetch(`${BASE_URL}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'include',
    body: JSON.stringify(payload),
  });
}

// === Переключение вкладок ===
function switchTab(name) {
  $$('.tab').forEach(t => t.classList.toggle('active', t.dataset.tab === name));
  $$('.form').forEach(f => f.classList.toggle('show', f.id.toLowerCase().includes(name)));
}

$$('[data-switch]')?.forEach(a => {
  a.addEventListener('click', (e) => {
    e.preventDefault();
    switchTab(e.currentTarget.getAttribute('data-switch'));
  });
});

// Клики по табам
$$('.tab').forEach(tab => {
  tab.addEventListener('click', () => switchTab(tab.dataset.tab));
});

// === ЛОГИН ===
$('#loginForm').addEventListener('submit', async (e) => {
  e.preventDefault();
  const email = $('#loginEmail').value.trim();
  const password = $('#loginPassword').value;

  // простая валидация
  setFieldError($('#loginEmail'), '');
  setFieldError($('#loginPassword'), '');

  let valid = true;
  if (!validateEmail(email)) {
    setFieldError($('#loginEmail'), 'Введите корректный email');
    valid = false;
  }
  if ((password || '').length < 6) {
    setFieldError($('#loginPassword'), 'Минимум 6 символов');
    valid = false;
  }
  if (!valid) return;

  const btn = $('#loginSubmit');
  setButtonLoading(btn, true);
  try {
    const res = await apiPost('/Auth/LogIn', { email, password });
    const data = await res.json();
    if (!res.ok || data.success === false) {
      throw new Error(data?.message || 'Не удалось войти');
    }
    showToast('Вход выполнен', 'success');
    $('#statusBar').textContent = `Токен: ${data.token || '—'}  |  Email: ${data?.user?.email || email}`;
  } catch (err) {
    console.error(err);
    showToast(err.message || 'Ошибка входа', 'error');
  } finally {
    setButtonLoading(btn, false);
  }
});

// === РЕГИСТРАЦИЯ ===
$('#registerForm').addEventListener('submit', async (e) => {
  e.preventDefault();
  const email = $('#regEmail').value.trim();
  const password = $('#regPassword').value;
  const password2 = $('#regPassword2').value;

  // очистка ошибок
  setFieldError($('#regEmail'), '');
  setFieldError($('#regPassword'), '');
  setFieldError($('#regPassword2'), '');

  let valid = true;
  if (!validateEmail(email)) {
    setFieldError($('#regEmail'), 'Введите корректный email');
    valid = false;
  }
  if ((password || '').length < 6) {
    setFieldError($('#regPassword'), 'Минимум 6 символов');
    valid = false;
  }
  if (password !== password2) {
    setFieldError($('#regPassword2'), 'Пароли не совпадают');
    valid = false;
  }
  if (!valid) return;

  // На сервер отправляем ТОЛЬКО email и password
  const payload = { email, password };

  const btn = $('#registerSubmit');
  setButtonLoading(btn, true);
  try {
    const res = await apiPost('/Auth/Register', payload);
    const data = await res.json();
    if (!res.ok || data.success === false) {
      throw new Error(data?.message || 'Регистрация не удалась');
    }
    showToast('Аккаунт создан! Теперь войдите', 'success');
    switchTab('login');
    $('#loginEmail').value = email; // подсказка юзеру
  } catch (err) {
    console.error(err);
    showToast(err.message || 'Ошибка регистрации', 'error');
  } finally {
    setButtonLoading(btn, false);
  }
});

// Покажем текущий режим в статусе
window.addEventListener('DOMContentLoaded', () => {
  const mode = USE_MOCK ? 'Локальный мок' : `API ${BASE_URL || '(текущий домен)'}`;
  $('#statusBar').textContent = `Режим: ${mode}`;
});