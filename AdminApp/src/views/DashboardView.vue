<template>
  <div class="dashboard">
    <h2>Dashboard</h2>
    <p>Vítej, {{ auth.user?.name }}.</p>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
const auth = useAuthStore()
</script>

<style scoped>
.dashboard {
  padding: 2rem;
}
</style>
