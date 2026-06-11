<script setup>
import { genderText } from '../constants/genders';

defineProps({
  profiles: {
    type: Array,
    required: true
  }
});

defineEmits(['edit', 'delete']);
</script>

<template>
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
              <button type="button" class="small" @click="$emit('edit', profile)">編輯</button>
              <button type="button" class="small danger" @click="$emit('delete', profile)">刪除</button>
            </td>
          </tr>
          <tr v-if="profiles.length === 0">
            <td colspan="7" class="empty">目前沒有資料</td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>
