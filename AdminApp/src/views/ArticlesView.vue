<template>
  <div class="page">
    <div class="page-header">
      <h2>Články</h2>
      <Button label="Nový článek" icon="pi pi-plus" @click="router.push('/clanky/novy')" />
    </div>

    <DataTable
      :value="articles"
      :loading="loading"
      stripedRows
      size="small"
      lazy
      paginator
      :rows="pageSize"
      :totalRecords="totalRecords"
      :rowsPerPageOptions="[10, 20, 50]"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
      @page="onPage"
    >
      <template #paginatorstart>
        <span class="paginator-info">{{ paginatorInfo }}</span>
      </template>
      <template #paginatorend>
      </template>
      <Column field="dateCreated" header="Datum" style="width: 130px">
        <template #body="{ data }">{{ formatDate(data.dateCreated) }}</template>
      </Column>
      <Column field="title" header="Název">
        <template #body="{ data }">
          <div class="title-cell">
            <span class="article-title">{{ data.title }}</span>
            <span v-if="data.top" class="top-badge">TOP</span>
          </div>
        </template>
      </Column>
      <Column header="Top" style="width: 70px; text-align: center">
        <template #body="{ data }">
          <ToggleSwitch :modelValue="data.top" @update:modelValue="toggleTop(data)" />
        </template>
      </Column>
      <Column style="width: 90px; text-align: right">
        <template #body="{ data }">
          <div class="icon-actions">
            <button class="icon-action edit" @click="router.push(`/clanky/${data.id}`)" title="Upravit">
              <i class="pi pi-pencil"></i>
            </button>
            <button class="icon-action delete" @click="confirmDelete(data)" title="Smazat">
              <i class="pi pi-trash"></i>
            </button>
          </div>
        </template>
      </Column>
    </DataTable>

    <ConfirmDialog />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'
import ToggleSwitch from 'primevue/toggleswitch'
import ConfirmDialog from 'primevue/confirmdialog'
import api from '@/services/api'

interface ArticleRow {
  id: string; title: string; dateCreated: string; top: boolean; urlId: number; hasImage: boolean
}

const router = useRouter()
const confirm = useConfirm()
const toast = useToast()

const articles = ref<ArticleRow[]>([])
const loading = ref(false)
const totalRecords = ref(0)
const pageSize = ref(10)
const currentPage = ref(1)
const sortOrder = ref('desc')

const paginatorInfo = computed(() => {
  if (totalRecords.value === 0) return '0 záznamů'
  const first = (currentPage.value - 1) * pageSize.value + 1
  const last = Math.min(currentPage.value * pageSize.value, totalRecords.value)
  return `${first}–${last} z ${totalRecords.value}`
})

async function loadArticles() {
  loading.value = true
  const { data } = await api.get<{ total: number; items: ArticleRow[] }>('/articles', {
    params: { page: currentPage.value, pageSize: pageSize.value, sortOrder: sortOrder.value },
  })
  articles.value = data.items
  totalRecords.value = data.total
  loading.value = false
}

function onPage(event: { page: number; rows: number }) {
  currentPage.value = event.page + 1
  pageSize.value = event.rows
  loadArticles()
}

function formatDate(d: string) {
  return new Date(d).toLocaleDateString('cs-CZ')
}

async function toggleTop(article: ArticleRow) {
  article.top = !article.top
  await api.patch(`/articles/${article.id}/top`, { top: article.top })
}

function confirmDelete(article: ArticleRow) {
  confirm.require({
    message: `Smazat článek „${article.title}"?`,
    header: 'Potvrzení',
    icon: 'pi pi-trash',
    rejectLabel: 'Zrušit',
    acceptLabel: 'Smazat',
    accept: async () => {
      await api.delete(`/articles/${article.id}`)
      toast.add({ severity: 'success', summary: 'Smazáno', life: 2000 })
      loadArticles()
    },
  })
}

onMounted(loadArticles)
</script>

<style scoped>
.article-title {
  font-weight: 500;
}

.title-cell {
  display: flex;
  align-items: center;
  gap: 8px;
}

.top-badge {
  display: inline-flex;
  align-items: center;
  padding: 2px 8px;
  border-radius: 99px;
  font-size: 0.69rem;
  font-weight: 700;
  background: var(--green-light);
  color: #5a7a10;
  letter-spacing: 0.03em;
  flex-shrink: 0;
}
</style>
