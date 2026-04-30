<template>
  <div class="page">
    <div class="page-header">
      <h2>Zápasy</h2>
      <Button label="Přidat zápas" icon="pi pi-plus" @click="openNew" />
    </div>

    <DataTable
      :value="games"
      :loading="loading"
      sortField="gameDate"
      :sortOrder="-1"
      stripedRows
      size="small"
      lazy
      paginator
      :rows="pageSize"
      :totalRecords="totalRecords"
      :rowsPerPageOptions="[10, 20, 50]"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown CurrentPageReport"
      currentPageReportTemplate="{first}–{last} z {totalRecords}"
      @page="onPage"
      @sort="onSort"
    >
      <Column field="gameDate" header="Datum" sortable style="width: 110px">
        <template #body="{ data }">{{ formatDate(data.gameDate) }}</template>
      </Column>
      <Column field="categoryTitle" header="Kategorie" sortable style="width: 110px" />
      <Column field="league" header="Liga" sortable style="width: 160px" />
      <Column header="Domácí" style="width: 180px">
        <template #body="{ data }">
          <span :class="{ 'winner': isHomeWinner(data) }">{{ data.homeTeam }}</span>
        </template>
      </Column>
      <Column header="Skóre" style="width: 80px; text-align: center">
        <template #body="{ data }">
          <span v-if="data.homeScore != null" class="score">{{ data.homeScore }}:{{ data.awayScore }}</span>
          <span v-else class="score-empty">—</span>
        </template>
      </Column>
      <Column header="Hosté" style="width: 180px">
        <template #body="{ data }">
          <span :class="{ 'winner': isAwayWinner(data) }">{{ data.awayTeam }}</span>
        </template>
      </Column>
      <Column style="width: 90px; text-align: right">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" text size="small" @click="openEdit(data)" />
          <Button icon="pi pi-trash" text size="small" severity="danger" @click="confirmDelete(data)" />
        </template>
      </Column>
    </DataTable>

    <!-- Dialog -->
    <Dialog v-model:visible="dialogVisible" :header="editingGame ? 'Upravit zápas' : 'Nový zápas'" modal style="width: 480px">
      <div class="form">
        <div class="form-row">
          <label>Kategorie</label>
          <Select v-model="form.categoryId" :options="categories" optionLabel="title" optionValue="id" placeholder="Vyberte kategorii" fluid />
        </div>
        <div class="form-row">
          <label>Datum</label>
          <DatePicker v-model="form.gameDate" dateFormat="dd.mm.yy" fluid />
        </div>
        <div class="form-row">
          <label>Liga</label>
          <AutoComplete
            v-model="form.league"
            :suggestions="leagueSuggestions"
            @complete="filterLeagues"
            dropdown
            fluid
            forceSelection="false"
          />
        </div>
        <div class="form-row-2">
          <div>
            <label>Domácí</label>
            <InputText v-model="form.homeTeam" fluid />
          </div>
          <div>
            <label>Hosté</label>
            <InputText v-model="form.awayTeam" fluid />
          </div>
        </div>
        <div class="form-row-2">
          <div>
            <label>Skóre domácí</label>
            <InputNumber v-model="form.homeScore" :min="0" fluid />
          </div>
          <div>
            <label>Skóre hosté</label>
            <InputNumber v-model="form.awayScore" :min="0" fluid />
          </div>
        </div>
      </div>
      <template #footer>
        <Button label="Zrušit" text @click="dialogVisible = false" />
        <Button :label="editingGame ? 'Uložit' : 'Přidat'" icon="pi pi-check" :loading="saving" @click="save" />
      </template>
    </Dialog>

    <ConfirmDialog />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import Select from 'primevue/select'
import DatePicker from 'primevue/datepicker'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import AutoComplete from 'primevue/autocomplete'
import ConfirmDialog from 'primevue/confirmdialog'
import api from '@/services/api'

interface Category { id: string; code: string; title: string }
interface Game {
  id: string; categoryId: string; categoryTitle: string; categoryCode: string
  gameDate: string; league: string; homeTeam: string; awayTeam: string
  homeScore: number | null; awayScore: number | null
}

const confirm = useConfirm()
const toast = useToast()

const games = ref<Game[]>([])
const categories = ref<Category[]>([])
const allLeagues = ref<string[]>([])
const loading = ref(false)
const dialogVisible = ref(false)
const saving = ref(false)
const editingGame = ref<Game | null>(null)
const leagueSuggestions = ref<string[]>([])
const totalRecords = ref(0)
const pageSize = ref(20)
const currentPage = ref(1)
const sortField = ref('gameDate')
const sortOrder = ref('desc')

function filterLeagues(event: { query: string }) {
  const q = event.query.toLowerCase()
  leagueSuggestions.value = q ? allLeagues.value.filter(l => l.toLowerCase().includes(q)) : allLeagues.value
}

async function loadGames() {
  loading.value = true
  const { data } = await api.get<{ total: number; items: Game[] }>('/games', {
    params: { page: currentPage.value, pageSize: pageSize.value, sortField: sortField.value, sortOrder: sortOrder.value },
  })
  games.value = data.items
  totalRecords.value = data.total
  loading.value = false
}

async function loadLeagues() {
  const { data } = await api.get<{ total: number; items: Game[] }>('/games', {
    params: { page: 1, pageSize: 9999, sortField: 'league', sortOrder: 'asc' },
  })
  allLeagues.value = [...new Set(data.items.map(g => g.league))].sort()
}

function onPage(event: { page: number; rows: number }) {
  currentPage.value = event.page + 1
  pageSize.value = event.rows
  loadGames()
}

function onSort(event: { sortField: string; sortOrder: number }) {
  sortField.value = event.sortField
  sortOrder.value = event.sortOrder === 1 ? 'asc' : 'desc'
  currentPage.value = 1
  loadGames()
}

const emptyForm = () => ({
  categoryId: '' as string,
  gameDate: null as Date | null,
  league: '',
  homeTeam: '',
  awayTeam: '',
  homeScore: null as number | null,
  awayScore: null as number | null,
})
const form = ref(emptyForm())

onMounted(async () => {
  const [, c] = await Promise.all([loadGames(), api.get<Category[]>('/categories'), loadLeagues()])
  categories.value = c.data
})

function formatDate(d: string) {
  return new Date(d).toLocaleDateString('cs-CZ')
}

function isHomeWinner(g: Game) {
  return g.homeScore != null && g.awayScore != null && g.homeScore > g.awayScore
}

function isAwayWinner(g: Game) {
  return g.homeScore != null && g.awayScore != null && g.awayScore > g.homeScore
}

function openNew() {
  editingGame.value = null
  form.value = emptyForm()
  dialogVisible.value = true
}

function openEdit(game: Game) {
  editingGame.value = game
  form.value = {
    categoryId: game.categoryId,
    gameDate: new Date(game.gameDate),
    league: game.league,
    homeTeam: game.homeTeam,
    awayTeam: game.awayTeam,
    homeScore: game.homeScore,
    awayScore: game.awayScore,
  }
  dialogVisible.value = true
}

async function save() {
  if (!form.value.categoryId || !form.value.gameDate || !form.value.league || !form.value.homeTeam || !form.value.awayTeam) {
    toast.add({ severity: 'warn', summary: 'Chybí údaje', detail: 'Vyplňte kategorii, datum, ligu a týmy.', life: 3000 })
    return
  }

  saving.value = true
  try {
    const payload = {
      categoryId: form.value.categoryId,
      gameDate: form.value.gameDate.toISOString().split('T')[0],
      league: form.value.league,
      homeTeam: form.value.homeTeam,
      awayTeam: form.value.awayTeam,
      homeScore: form.value.homeScore,
      awayScore: form.value.awayScore,
    }

    if (editingGame.value) {
      await api.put(`/games/${editingGame.value.id}`, payload)
      toast.add({ severity: 'success', summary: 'Uloženo', life: 2000 })
    } else {
      await api.post('/games', payload)
      toast.add({ severity: 'success', summary: 'Přidáno', life: 2000 })
    }
    dialogVisible.value = false
    await Promise.all([loadGames(), loadLeagues()])
  } finally {
    saving.value = false
  }
}

function confirmDelete(game: Game) {
  confirm.require({
    message: `Smazat zápas ${game.homeTeam} vs ${game.awayTeam}?`,
    header: 'Potvrzení',
    icon: 'pi pi-trash',
    rejectLabel: 'Zrušit',
    acceptLabel: 'Smazat',
    acceptSeverity: 'danger',
    accept: async () => {
      await api.delete(`/games/${game.id}`)
      toast.add({ severity: 'success', summary: 'Smazáno', life: 2000 })
      loadGames()
    },
  })
}
</script>

<style scoped>
.page {
  padding: 2rem;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

h2 {
  font-size: 1.4rem;
  font-weight: 700;
  color: #1a1a1a;
}

.winner {
  font-weight: 700;
  color: #4a7c10;
}

.score {
  font-weight: 700;
  font-variant-numeric: tabular-nums;
}

.score-empty {
  color: #bbb;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding-top: 0.5rem;
}

.form-row {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.form-row-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.form-row-2 > div {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

label {
  font-size: 0.825rem;
  font-weight: 600;
  color: #555;
}
</style>
