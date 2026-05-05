<template>
  <div class="admin-layout">
    <div v-if="isSidebarOpen" class="mobile-overlay" @click="closeSidebar"></div>

    <aside id="admin-sidebar" :class="['sidebar', { 'sidebar-open': isSidebarOpen }]">
      <div class="sidebar-logo-area">
        <div class="logo-icon">
          <svg width="18" height="18" viewBox="0 0 20 20" fill="none">
            <circle cx="10" cy="10" r="8" stroke="white" stroke-width="1.5"/>
            <path d="M5.5 5.5c1 1.5 1 5 0 9M14.5 5.5c-1 1.5-1 5 0 9" stroke="white" stroke-width="1.2" stroke-linecap="round"/>
            <path d="M6 7.5q2 1 4 0M6 12.5q2-1 4 0M8 9q2 1 4 0M8 11q2-1 4 0" stroke="white" stroke-width="0.9" stroke-linecap="round"/>
          </svg>
        </div>
        <div>
          <div class="logo-name">Miners</div>
          <div class="logo-sub">Admin</div>
        </div>
      </div>

      <nav class="sidebar-nav">
        <RouterLink
          v-for="item in menuItems"
          :key="item.to"
          :to="item.to"
          :class="['nav-item', { active: isItemActive(item) }]"
          @click="closeSidebar"
        >
          <i :class="[item.icon, { 'nav-icon-active': isItemActive(item) }]" class="nav-icon"></i>
          <span>{{ item.label }}</span>
        </RouterLink>
      </nav>

      <div class="sidebar-footer">
        <div class="user-avatar-wrap">
          <img v-if="auth.user?.picture" :src="auth.user.picture" class="user-avatar" alt="User avatar" />
          <div v-else class="user-avatar user-avatar-placeholder">{{ initials }}</div>
        </div>
        <div class="user-texts">
          <div class="user-name">{{ auth.user?.name }}</div>
          <div class="user-email">{{ auth.user?.email }}</div>
        </div>
        <button class="logout-btn" @click="handleLogout" title="Odhlásit">
          <i class="pi pi-sign-out"></i>
        </button>
      </div>
    </aside>

    <main class="content">
      <header class="mobile-header">
        <button
          class="mobile-menu-btn"
          type="button"
          aria-label="Otevřít menu"
          aria-controls="admin-sidebar"
          :aria-expanded="isSidebarOpen"
          @click="openSidebar"
        >
          <i class="pi pi-bars"></i>
        </button>
        <span class="mobile-title">Admin</span>
      </header>
      <RouterView />
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, onBeforeUnmount, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const route = useRoute()
const router = useRouter()
const auth = useAuthStore()
const isSidebarOpen = ref(false)

const menuItems = [
  { to: '/', label: 'Dashboard', icon: 'pi pi-home' },
  { to: '/zapasy', label: 'Zápasy', icon: 'pi pi-calendar' },
  { to: '/clanky', label: 'Články', icon: 'pi pi-file-edit' },
  { to: '/scoreboard', label: 'Scoreboard', icon: 'pi pi-chart-line' },
]

function isItemActive(item: { to: string }) {
  if (item.to === '/') return route.path === '/'
  return route.path.startsWith(item.to)
}

const initials = computed(() => {
  const name = auth.user?.name ?? ''
  return name.split(' ').map(w => w[0]).join('').slice(0, 2).toUpperCase()
})

function handleLogout() {
  auth.logout()
  router.push('/login')
}

function openSidebar() {
  isSidebarOpen.value = true
}

function closeSidebar() {
  isSidebarOpen.value = false
}

watch(
  () => route.fullPath,
  () => {
    isSidebarOpen.value = false
  }
)

watch(isSidebarOpen, (open) => {
  document.body.style.overflow = open ? 'hidden' : ''
})

onBeforeUnmount(() => {
  document.body.style.overflow = ''
})
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

.mobile-overlay {
  display: none;
}

.sidebar {
  width: var(--sidebar-width);
  background: var(--sidebar-bg);
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
  position: sticky;
  top: 0;
  height: 100vh;
}

.mobile-header {
  display: none;
}

/* Logo */
.sidebar-logo-area {
  padding: 20px 18px 18px;
  border-bottom: 1px solid #22262e;
  display: flex;
  align-items: center;
  gap: 10px;
}

.logo-icon {
  width: 34px;
  height: 34px;
  border-radius: 9px;
  background: var(--green);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.logo-name {
  font-weight: 700;
  font-size: 0.85rem;
  color: #fff;
  letter-spacing: 0.01em;
  line-height: 1.2;
}

.logo-sub {
  font-size: 0.65rem;
  color: #6b7280;
  font-weight: 500;
  letter-spacing: 0.05em;
  text-transform: uppercase;
}

/* Nav */
.sidebar-nav {
  flex: 1;
  padding: 12px 0;
  display: flex;
  flex-direction: column;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 9px 18px;
  font-size: 0.845rem;
  font-weight: 400;
  color: #8a9099;
  text-decoration: none;
  transition: all 0.12s;
  border-left: 3px solid transparent;
}

.nav-item:hover {
  background: #181c23;
  color: #c8cdd4;
}

.nav-item.active {
  background: #1e2229;
  color: #fff;
  font-weight: 600;
  border-left-color: var(--green);
}

.nav-icon {
  font-size: 0.9rem;
  color: inherit;
}

.nav-item.active .nav-icon {
  color: var(--green);
}

/* Footer */
.sidebar-footer {
  border-top: 1px solid #22262e;
  padding: 14px 18px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  flex-shrink: 0;
  object-fit: cover;
}

.user-avatar-placeholder {
  background: linear-gradient(135deg, #93C11F, #5a7a10);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.75rem;
  font-weight: 700;
  color: #fff;
}

.user-texts {
  flex: 1;
  min-width: 0;
}

.user-name {
  font-size: 0.78rem;
  font-weight: 600;
  color: #e5e7eb;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.user-email {
  font-size: 0.69rem;
  color: #6b7280;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.logout-btn {
  background: none;
  border: none;
  cursor: pointer;
  color: #6b7280;
  display: flex;
  align-items: center;
  padding: 4px;
  border-radius: 6px;
  transition: color 0.15s;
  font-size: 0.9rem;
}

.logout-btn:hover {
  color: #e5e7eb;
}

.content {
  flex: 1;
  background: #f4f5f7;
  overflow-y: auto;
  min-height: 100vh;
  min-width: 0;
}

@media (max-width: 900px) {
  .mobile-overlay {
    display: block;
    position: fixed;
    inset: 0;
    background: rgba(8, 12, 20, 0.5);
    z-index: 39;
  }

  .sidebar {
    position: fixed;
    top: 0;
    left: 0;
    height: 100dvh;
    z-index: 40;
    transform: translateX(-100%);
    transition: transform 0.2s ease;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.35);
  }

  .sidebar-open {
    transform: translateX(0);
  }

  .mobile-header {
    position: sticky;
    top: 0;
    z-index: 10;
    display: flex;
    align-items: center;
    gap: 0.75rem;
    height: 56px;
    padding: 0 12px;
    background: #f4f5f7;
    border-bottom: 1px solid #dde1e7;
  }

  .mobile-menu-btn {
    width: 36px;
    height: 36px;
    border: 1px solid #cfd5dd;
    border-radius: 8px;
    background: #fff;
    color: #1f2937;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    font-size: 0.95rem;
  }

  .mobile-title {
    font-size: 0.95rem;
    font-weight: 600;
    color: #1f2937;
  }
}
</style>
