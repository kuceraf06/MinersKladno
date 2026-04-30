<template>
  <div class="admin-layout">
    <aside class="sidebar">
      <div class="sidebar-header">
        <span class="sidebar-logo">⚾</span>
        <span class="sidebar-title">Miners Admin</span>
      </div>
      <nav>
        <ul>
          <li v-for="item in menuItems" :key="item.to">
            <RouterLink :to="item.to" class="menu-item" active-class="active">
              <i :class="item.icon"></i>
              <span>{{ item.label }}</span>
            </RouterLink>
          </li>
        </ul>
      </nav>
      <div class="sidebar-footer">
        <div class="user-info">
          <img v-if="auth.user?.picture" :src="auth.user.picture" class="avatar" />
          <div class="user-texts">
            <div class="user-name">{{ auth.user?.name }}</div>
            <div class="user-email">{{ auth.user?.email }}</div>
          </div>
        </div>
        <Button icon="pi pi-sign-out" text severity="secondary" @click="handleLogout" v-tooltip="'Odhlásit'" />
      </div>
    </aside>
    <main class="content">
      <RouterView />
    </main>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import Button from 'primevue/button'

const router = useRouter()
const auth = useAuthStore()

const menuItems = [
  { to: '/', label: 'Dashboard', icon: 'pi pi-home' },
  { to: '/zapasy', label: 'Zápasy', icon: 'pi pi-calendar' },
]

function handleLogout() {
  auth.logout()
  router.push('/login')
}
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

.sidebar {
  width: 240px;
  background: #1a1a1a;
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
}

.sidebar-header {
  padding: 1.25rem 1.5rem;
  border-bottom: 1px solid #2e2e2e;
  display: flex;
  align-items: center;
  gap: 0.625rem;
}

.sidebar-logo {
  font-size: 1.4rem;
  line-height: 1;
}

.sidebar-title {
  font-weight: 700;
  font-size: 1.05rem;
  color: #93C11F;
  letter-spacing: 0.02em;
}

nav ul {
  list-style: none;
  padding: 0.75rem 0;
  margin: 0;
}

.menu-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.7rem 1.5rem;
  color: #aaa;
  text-decoration: none;
  font-size: 0.9rem;
  transition: background 0.15s, color 0.15s;
  border-left: 3px solid transparent;
}

.menu-item:hover {
  background: #2a2a2a;
  color: #eee;
}

.menu-item.active {
  background: #2a2a2a;
  color: #93C11F;
  border-left-color: #93C11F;
}

.sidebar-footer {
  margin-top: auto;
  padding: 1rem 1.25rem;
  border-top: 1px solid #2e2e2e;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex: 1;
  min-width: 0;
}

.avatar {
  width: 34px;
  height: 34px;
  border-radius: 50%;
  border: 2px solid #93C11F;
  flex-shrink: 0;
}

.user-texts {
  min-width: 0;
}

.user-name {
  font-size: 0.8rem;
  font-weight: 600;
  color: #eee;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-email {
  font-size: 0.7rem;
  color: #777;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.content {
  flex: 1;
  background: #f5f5f5;
  overflow-y: auto;
}
</style>
