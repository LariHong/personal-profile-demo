import { onMounted, reactive, ref } from 'vue';
import {
  createProfile,
  deleteProfile,
  searchProfiles,
  updateProfile
} from '../api/profiles';

export function createEmptyProfileForm() {
  return {
    nationalId: '',
    name: '',
    gender: '',
    birthday: '',
    city: '',
    district: '',
    address: '',
    phone: ''
  };
}

export function useProfiles() {
  const profiles = ref([]);
  const keyword = ref('');
  const editingId = ref(null);
  const form = reactive(createEmptyProfileForm());
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
      profiles.value = await searchProfiles(keyword.value);
    } catch (error) {
      showMessage(error.message, 'error');
    } finally {
      loading.value = false;
    }
  }

  async function saveProfile() {
    message.value = '';

    try {
      if (editingId.value) {
        await updateProfile(editingId.value, form);
        showMessage('資料已更新。', 'success');
      } else {
        await createProfile(form);
        showMessage('資料已新增。', 'success');
      }

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

  async function removeProfile(profile) {
    try {
      await deleteProfile(profile.id);
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
    Object.assign(form, createEmptyProfileForm());

    if (clearMessage) {
      message.value = '';
    }
  }

  function showMessage(text, type) {
    message.value = text;
    messageType.value = type;
  }

  return {
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
  };
}
