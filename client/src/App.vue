<script setup>
import ProfileForm from './components/ProfileForm.vue';
import ProfileTable from './components/ProfileTable.vue';
import { useProfiles } from './composables/useProfiles';

const {
  profiles,
  keyword,
  editingId,
  form,
  message,
  messageType,
  loading,
  loadProfiles,
  saveProfile,
  editProfile,
  removeProfile,
  resetForm
} = useProfiles();

function confirmDelete(profile) {
  if (!confirm(`確定刪除 ${profile.name}？`)) {
    return;
  }

  removeProfile(profile);
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
      <ProfileForm
        :editing-id="editingId"
        :form="form"
        :message="message"
        :message-type="messageType"
        @submit="saveProfile"
        @reset="resetForm"
      />

      <ProfileTable
        :profiles="profiles"
        @edit="editProfile"
        @delete="confirmDelete"
      />
    </section>
  </main>
</template>
