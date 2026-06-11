<script setup>
import { GENDER_OPTIONS } from '../constants/genders';

defineProps({
  editingId: {
    type: Number,
    default: null
  },
  form: {
    type: Object,
    required: true
  },
  message: {
    type: String,
    default: ''
  },
  messageType: {
    type: String,
    default: 'info'
  }
});

defineEmits(['submit', 'reset']);
</script>

<template>
  <form class="editor" @submit.prevent="$emit('submit')">
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
        <option v-for="gender in GENDER_OPTIONS" :key="gender.value" :value="gender.value">
          {{ gender.label }}
        </option>
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
      <button type="button" class="secondary" @click="$emit('reset')">清空</button>
    </div>
  </form>
</template>
