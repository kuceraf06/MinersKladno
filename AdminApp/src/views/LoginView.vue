<template>
  <div class="login-page">
    <div class="login-card">
      <div class="login-logo">⚾</div>
      <h1>Miners <span class="green">Admin</span></h1>
      <p class="subtitle">Přihlaste se klubovým Google účtem</p>
      <div id="google-signin-btn"></div>
      <p v-if="error" class="error">
        <i class="pi pi-exclamation-triangle"></i>
        {{ error }}
      </p>
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
  background: #1a1a1a;
}

.login-card {
  background: #242424;
  padding: 3rem 2.5rem;
  border-radius: 14px;
  border: 1px solid #2e2e2e;
  text-align: center;
  width: 340px;
  box-shadow: 0 8px 40px rgba(0, 0, 0, 0.5);
}

.login-logo {
  font-size: 3.5rem;
  margin-bottom: 0.75rem;
  line-height: 1;
}

h1 {
  margin: 0 0 0.5rem;
  font-size: 1.8rem;
  font-weight: 700;
  color: #eee;
}

.green {
  color: #93C11F;
}

.subtitle {
  color: #777;
  font-size: 0.875rem;
  margin-bottom: 2rem;
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
</style>
