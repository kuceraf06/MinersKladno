<template>
  <div class="page">
	<div class="page-header">
	  <h2>Scoreboard</h2>
	</div>

	<div class="card">
    <form class="settings-form" @submit.prevent="save">
    <div class="form-row form-row--switch">
      <label for="active">Aktivní</label>
      <ToggleSwitch v-model="form.active" inputId="active" />
    </div>

    <div class="form-row">
      <label for="typ">Typ</label>
      <Select
      v-model="form.typ"
      inputId="typ"
      :options="typOptions"
      optionLabel="label"
      optionValue="value"
      class="field-control"
      />
    </div>

    <div class="form-row">
      <label for="homeTeam">Domácí</label>
      <InputText v-model.trim="form.homeTeam" inputId="homeTeam" class="field-control" />
    </div>

    <div class="form-row">
      <label for="visitorTeam">Hosté</label>
      <InputText v-model.trim="form.visitorTeam" inputId="visitorTeam" class="field-control" />
    </div>

    <div v-if="form.typ === 'B'" class="form-row">
      <label for="mbId">MbId</label>
      <InputText v-model.trim="form.mbId" inputId="mbId" class="field-control" />
    </div>

    <div class="actions form-row--actions">
      <Button label="Uložit" icon="pi pi-check" :loading="saving" type="submit" class="save-btn" />
    </div>
    </form>
	</div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import Select from 'primevue/select'
import ToggleSwitch from 'primevue/toggleswitch'
import { useToast } from 'primevue/usetoast'
import api from '@/services/api'

interface ScoreboardForm {
  active: boolean
  typ: 'M' | 'B'
  homeTeam: string
  visitorTeam: string
  mbId: string
}

const toast = useToast()
const saving = ref(false)
const typOptions = [
  { label: 'Manuální', value: 'M' },
  { label: 'MyBallClub', value: 'B' },
]
const form = ref<ScoreboardForm>({
  active: false,
  typ: 'M',
  homeTeam: '',
  visitorTeam: '',
  mbId: '',
})

async function loadScoreboard() {
  const { data } = await api.get<ScoreboardForm>('/scoreboard')
  form.value = {
	active: data.active,
  typ: data.typ === 'B' ? 'B' : 'M',
  homeTeam: data.homeTeam || '',
  visitorTeam: data.visitorTeam || '',
  mbId: data.mbId || '',
  }
}

async function save() {
  saving.value = true
  try {
	await api.put('/scoreboard', form.value)
	toast.add({ severity: 'success', summary: 'Uloženo', detail: 'Nastavení scoreboardu bylo uloženo.', life: 2500 })
  } finally {
	saving.value = false
  }
}

onMounted(loadScoreboard)
</script>

<style scoped>
.card {
  width: 100%;
  max-width: 700px;
  background: #fff;
  border-radius: 12px;
  border: 1.5px solid var(--border);
  padding: 20px 22px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
}

.settings-form {
  display: grid;
  gap: 0.25rem;
}

.form-row {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
  margin-bottom: 1rem;
}

.form-row--switch {
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  gap: 0.75rem;
}

label {
  font-size: 0.825rem;
  font-weight: 600;
  color: #555;
}

.field-control {
  width: 100%;
}

.actions {
  margin-top: 0.75rem;
}

.save-btn {
  min-width: 140px;
}

@media (max-width: 768px) {
  .card {
    padding: 14px;
    border-radius: 10px;
  }

  .form-row {
    margin-bottom: 0.8rem;
  }

  .form-row--actions {
    position: sticky;
    bottom: 0;
    background: #fff;
    padding-top: 0.5rem;
  }

  .save-btn {
    width: 100%;
  }
}
</style>

