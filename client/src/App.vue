<script setup>
import { onMounted, reactive, ref } from 'vue';

const emptyForm = () => ({
  nationalId: '',
  name: '',
  gender: '',
  birthday: '',
  city: '',
  district: '',
  address: '',
  phone: ''
});

const profiles = ref([]);
const keyword = ref('');
const editingId = ref(null);
const form = reactive(emptyForm());
const message = ref('');
const messageType = ref('info');
const loading = ref(false);

onMounted(() => {
  loadProfiles();
});

async function loadProfiles() {
  loading.value = true;
  message.value = '';

  try {
    const url = new URL('/api/profiles', window.location.origin);
    if (keyword.value.trim()) {
      url.searchParams.set('keyword', keyword.value.trim());
    }

    const response = await fetch(url);
    await ensureOk(response);
    profiles.value = await response.json();
  } catch (error) {
    showMessage(error.message, 'error');
  } finally {
    loading.value = false;
  }
}

async function saveProfile() {
  message.value = '';

  try {
    const url = editingId.value ? `/api/profiles/${editingId.value}` : '/api/profiles';
    const response = await fetch(url, {
      method: editingId.value ? 'PUT' : 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form)
    });

    await ensureOk(response);
    showMessage(editingId.value ? '資料已更新。' : '資料已新增。', 'success');
    resetForm(false);
    await loadProfiles();
  } catch (error) {
    showMessage(error.message, 'error');
  }
}

function editProfile(profile) {
  editingId.value = profile.id;
  Object.assign(form, {
    nationalId: profile.nationalId,
    name: profile.name,
    gender: profile.gender,
    birthday: profile.birthday,
    city: profile.city,
    district: profile.district,
    address: profile.address,
    phone: profile.phone
  });
  message.value = '';
}

async function deleteProfile(profile) {
  if (!confirm(`確定刪除 ${profile.name}？`)) {
    return;
  }

  try {
    const response = await fetch(`/api/profiles/${profile.id}`, { method: 'DELETE' });
    await ensureOk(response);
    showMessage('資料已刪除。', 'success');
    await loadProfiles();
    if (editingId.value === profile.id) {
      resetForm(false);
    }
  } catch (error) {
    showMessage(error.message, 'error');
  }
}

function resetForm(clearMessage = true) {
  editingId.value = null;
  Object.assign(form, emptyForm());
  if (clearMessage) {
    message.value = '';
  }
}

function genderText(gender) {
  return { Male: '男', Female: '女', Other: '其他' }[gender] ?? gender;
}

function showMessage(text, type) {
  message.value = text;
  messageType.value = type;
}

async function ensureOk(response) {
  if (response.ok) {
    return;
  }

  let payload = null;
  try {
    payload = await response.json();
  } catch {
    throw new Error(`HTTP ${response.status}`);
  }

  if (payload?.errors) {
    const details = Object.values(payload.errors).flat().join(' ');
    throw new Error(details || payload.title || `HTTP ${response.status}`);
  }

  throw new Error(payload?.detail || payload?.title || `HTTP ${response.status}`);
}
</script>

<template>
  <main class="app-shell">
    <section class="toolbar">
      <div>
        <h1>個人基本資料維護</h1>
        <p>ASP.NET Core 10 RESTful API + Vue3 + SQL Server</p>
      </div>
      <form class="search" @submit.prevent="loadProfiles">
        <input v-model.trim="keyword" type="search" placeholder="搜尋姓名、身分證、縣市、電話">
        <button type="submit" :disabled="loading">查詢</button>
        <button type="button" class="secondary" @click="resetForm()">新增</button>
      </form>
    </section>

    <section class="workspace">
      <form class="editor" @submit.prevent="saveProfile">
        <h2>{{ editingId ? '編輯資料' : '新增資料' }}</h2>

        <label>
          身分證字號
          <input v-model.trim="form.nationalId" maxlength="10" autocomplete="off" required>
        </label>

        <label>
          姓名
          <input v-model.trim="form.name" maxlength="80" required>
        </label>

        <label>
          性別
          <select v-model="form.gender" required>
            <option value="">請選擇</option>
            <option value="Male">男</option>
            <option value="Female">女</option>
            <option value="Other">其他</option>
          </select>
        </label>

        <label>
          生日
          <input v-model="form.birthday" type="date" required>
        </label>

        <div class="address-grid">
          <label>
            縣市
            <input v-model.trim="form.city" maxlength="20" required>
          </label>
          <label>
            鄉鎮市區
            <input v-model.trim="form.district" maxlength="20" required>
          </label>
        </div>

        <label>
          地址
          <input v-model.trim="form.address" maxlength="160" required>
        </label>

        <label>
          聯絡電話
          <input v-model.trim="form.phone" maxlength="30" required>
        </label>

        <div v-if="message" :class="['message', messageType]">{{ message }}</div>

        <div class="actions">
          <button type="submit">{{ editingId ? '儲存變更' : '新增資料' }}</button>
          <button type="button" class="secondary" @click="resetForm()">清空</button>
        </div>
      </form>

      <section class="list">
        <div class="list-header">
          <h2>資料列表</h2>
          <span>{{ profiles.length }} 筆</span>
        </div>

        <div class="table-wrap">
          <table>
            <thead>
              <tr>
                <th>身分證</th>
                <th>姓名</th>
                <th>性別</th>
                <th>生日</th>
                <th>地址</th>
                <th>電話</th>
                <th>操作</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="profile in profiles" :key="profile.id">
                <td>{{ profile.nationalId }}</td>
                <td>{{ profile.name }}</td>
                <td>{{ genderText(profile.gender) }}</td>
                <td>{{ profile.birthday }}</td>
                <td>{{ profile.city }} {{ profile.district }} {{ profile.address }}</td>
                <td>{{ profile.phone }}</td>
                <td class="row-actions">
                  <button type="button" class="small" @click="editProfile(profile)">編輯</button>
                  <button type="button" class="small danger" @click="deleteProfile(profile)">刪除</button>
                </td>
              </tr>
              <tr v-if="profiles.length === 0">
                <td colspan="7" class="empty">目前沒有資料</td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </section>
  </main>
</template>
