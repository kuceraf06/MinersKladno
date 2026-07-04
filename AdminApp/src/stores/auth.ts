import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/services/api'

interface User {
  email: string
  name: string
  picture: string
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('jwt'))
  const user = ref<User | null>(JSON.parse(localStorage.getItem('user') ?? 'null'))

  const isAuthenticated = computed(() => !!token.value)

  async function loginWithGoogle(googleToken: string) {
    const response = await api.post<{ token: string; user: User }>('/auth/google', { token: googleToken })
    token.value = response.data.token
    user.value = response.data.user
    localStorage.setItem('jwt', response.data.token)
    localStorage.setItem('user', JSON.stringify(response.data.user))
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('jwt')
    localStorage.removeItem('user')
  }

  return { token, user, isAuthenticated, loginWithGoogle, logout }
})
