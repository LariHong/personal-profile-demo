const PROFILES_ENDPOINT = '/api/profiles';

export async function searchProfiles(keyword) {
  const url = new URL(PROFILES_ENDPOINT, window.location.origin);
  const term = keyword.trim();

  if (term) {
    url.searchParams.set('keyword', term);
  }

  const response = await fetch(url);
  await ensureOk(response);
  return response.json();
}

export async function createProfile(profile) {
  const response = await fetch(PROFILES_ENDPOINT, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(profile)
  });

  await ensureOk(response);
  return response.json();
}

export async function updateProfile(id, profile) {
  const response = await fetch(`${PROFILES_ENDPOINT}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(profile)
  });

  await ensureOk(response);
  return response.json();
}

export async function deleteProfile(id) {
  const response = await fetch(`${PROFILES_ENDPOINT}/${id}`, { method: 'DELETE' });
  await ensureOk(response);
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
