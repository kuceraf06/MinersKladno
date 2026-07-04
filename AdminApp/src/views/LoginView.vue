<template>
  <div class="login-page">
    <div class="login-card">
      <div class="login-logo-icon">
        <svg width="28" height="28" viewBox="0 0 20 20" fill="none">
          <circle cx="10" cy="10" r="8" stroke="white" stroke-width="1.5"/>
          <path d="M5.5 5.5c1 1.5 1 5 0 9M14.5 5.5c-1 1.5-1 5 0 9" stroke="white" stroke-width="1.2" stroke-linecap="round"/>
          <path d="M6 7.5q2 1 4 0M6 12.5q2-1 4 0M8 9q2 1 4 0M8 11q2-1 4 0" stroke="white" stroke-width="0.9" stroke-linecap="round"/>
        </svg>
      </div>
      <h1>Miners <span class="green">Admin</span></h1>
      <p class="subtitle">Přihlaste se klubovým Google účtem</p>
      <div id="google-signin-btn"></div>
      <p v-if="error" class="error">
        <i class="pi pi-exclamation-triangle"></i>
        {{ error }}
      </p>
      <p class="hint">Přístup pouze pro členy klubu</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()
const error = ref('')

function loadGsiScript(): Promise<void> {
  return new Promise((resolve, reject) => {
    if (document.getElementById('google-gsi')) { resolve(); return }
    const script = document.createElement('script')
    script.id = 'google-gsi'
    script.src = 'https://accounts.google.com/gsi/client'
    script.onload = () => resolve()
    script.onerror = () => reject(new Error('Google GSI script se nepodařilo načíst'))
    document.head.appendChild(script)
  })
}

async function handleCredentialResponse(response: google.accounts.id.CredentialResponse) {
  error.value = ''
  try {
    await auth.loginWithGoogle(response.credential)
    router.push('/')
  } catch {
    error.value = 'Přístup odepřen. Váš účet nemá oprávnění k administraci.'
  }
}

onMounted(async () => {
  await loadGsiScript()
  google.accounts.id.initialize({
    client_id: import.meta.env.VITE_GOOGLE_CLIENT_ID,
    callback: handleCredentialResponse,
  })
  google.accounts.id.renderButton(document.getElementById('google-signin-btn')!, {
    type: 'standard',
    theme: 'outline',
    size: 'large',
    text: 'signin_with',
    locale: 'cs',
    width: 280,
  })
})
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #111318 60%, #1a2010);
}

.login-card {
  width: 360px;
  background: #181c23;
  border-radius: 18px;
  border: 1px solid #22262e;
  padding: 44px 40px;
  box-shadow: 0 24px 60px rgba(0, 0, 0, .5);
  text-align: center;
}

.login-logo-icon {
  width: 60px;
  height: 60px;
  border-radius: 16px;
  background: var(--green);
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
}

h1 {
  margin: 0 0 6px;
  font-size: 1.5rem;
  font-weight: 700;
  color: #fff;
}

.green {
  color: var(--green);
}

.subtitle {
  color: #6b7280;
  font-size: 0.845rem;
  margin: 0 0 32px;
}

#google-signin-btn {
  display: flex;
  justify-content: center;
}

.error {
  margin-top: 1.25rem;
  color: #ff6b6b;
  font-size: 0.825rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.4rem;
}

.hint {
  font-size: 0.72rem;
  color: #4b5563;
  margin-top: 20px;
}
</style>
