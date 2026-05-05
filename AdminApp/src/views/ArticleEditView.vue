<template>
  <div class="editor-page">
    <div class="editor-header">
      <Button icon="pi pi-arrow-left" text label="Zpět" @click="router.push('/clanky')" />
      <h2>{{ isNew ? 'Nový článek' : 'Upravit článek' }}</h2>
      <div class="header-actions">
        <ToggleSwitch v-model="form.top" inputId="top" />
        <label for="top">Top</label>
        <Button label="Uložit" icon="pi pi-check" :loading="saving" @click="save" />
      </div>
    </div>

    <div class="editor-body" v-if="!loading">
      <div class="meta-row">
        <InputText v-model="form.title" placeholder="Nadpis článku" class="title-input" />
      </div>

      <div class="image-row">
        <div class="image-thumb" :class="{ 'has-image': !!imageUrl }" @click="triggerCoverUpload">
          <img v-if="imageUrl" :src="imageUrl" alt="Obrázek článku" />
          <template v-else>
            <i class="pi pi-upload" style="color: var(--text-muted); font-size: 0.9rem"></i>
            <span>foto</span>
          </template>
        </div>
        <div class="image-info">
          <div class="image-label">Titulní obrázek</div>
          <div class="image-hint">JPG, PNG, WebP · max 5 MB</div>
        </div>
        <input
          ref="coverImageInput"
          type="file"
          accept="image/*"
          style="display: none"
          @change="onCoverImageChanged"
        />
      </div>

      <!-- Toolbar -->
      <div class="toolbar" v-if="editor">
        <div class="toolbar-group">
          <button @click="editor.chain().focus().toggleBold().run()" :class="{ active: editor.isActive('bold') }" title="Tučné" style="font-weight:700">B</button>
          <button @click="editor.chain().focus().toggleItalic().run()" :class="{ active: editor.isActive('italic') }" title="Kurzíva" style="font-style:italic">I</button>
          <button @click="editor.chain().focus().toggleUnderline().run()" :class="{ active: editor.isActive('underline') }" title="Podtržení" style="text-decoration:underline">U</button>
        </div>
        <div class="toolbar-group">
          <button @click="editor.chain().focus().toggleHeading({ level: 2 }).run()" :class="{ active: editor.isActive('heading', { level: 2 }) }" title="Nadpis 2">H2</button>
          <button @click="editor.chain().focus().toggleHeading({ level: 3 }).run()" :class="{ active: editor.isActive('heading', { level: 3 }) }" title="Nadpis 3">H3</button>
        </div>
        <div class="toolbar-group">
          <button @click="editor.chain().focus().toggleBulletList().run()" :class="{ active: editor.isActive('bulletList') }" title="Seznam">
            <i class="pi pi-list" />
          </button>
          <button @click="editor.chain().focus().toggleOrderedList().run()" :class="{ active: editor.isActive('orderedList') }" title="Číslovaný seznam">
            <i class="pi pi-sort-numeric-down" />
          </button>
          <button @click="editor.chain().focus().toggleBlockquote().run()" :class="{ active: editor.isActive('blockquote') }" title="Citace">
            <i class="pi pi-comment" />
          </button>
        </div>
        <div class="toolbar-group">
          <button @click="setLink" :class="{ active: editor.isActive('link') }" title="Odkaz">
            <i class="pi pi-link" />
          </button>
          <button @click="editor.chain().focus().unsetLink().run()" v-if="editor.isActive('link')" title="Odebrat odkaz">
            <i class="pi pi-times" />
          </button>
        </div>
        <div class="toolbar-group">
          <button @click="editor.chain().focus().setTextAlign('left').run()" :class="{ active: editor.isActive({ textAlign: 'left' }) }" title="Vlevo">
            <i class="pi pi-align-left" />
          </button>
          <button @click="editor.chain().focus().setTextAlign('center').run()" :class="{ active: editor.isActive({ textAlign: 'center' }) }" title="Na střed">
            <i class="pi pi-align-center" />
          </button>
          <button @click="editor.chain().focus().setTextAlign('right').run()" :class="{ active: editor.isActive({ textAlign: 'right' }) }" title="Vpravo">
            <i class="pi pi-align-right" />
          </button>
        </div>
        <div class="toolbar-group">
          <button @click="triggerInlineImageUpload" :disabled="uploadingInlineImage" title="Vložit obrázek do textu">
            <i class="pi pi-image" />
          </button>
          <button @click="editor.chain().focus().undo().run()" :disabled="!editor.can().undo()" title="Zpět">
            <i class="pi pi-undo" />
          </button>
          <button @click="editor.chain().focus().redo().run()" :disabled="!editor.can().redo()" title="Vpřed">
            <i class="pi pi-refresh" />
          </button>
        </div>
        <div class="toolbar-group" style="margin-left: auto">
          <button @click="toggleHtmlMode" :class="{ active: htmlMode }" title="Zobrazit HTML">
            <i class="pi pi-code" />
          </button>
        </div>
      </div>

      <input
        ref="inlineImageInput"
        type="file"
        accept="image/*"
        style="display: none"
        @change="onInlineImageSelected"
      />

      <EditorContent v-if="!htmlMode" :editor="editor" class="tiptap-wrapper" />
      <textarea v-else v-model="rawHtml" class="html-textarea" spellcheck="false" />
    </div>

    <div v-else class="loading-state">
      <ProgressSpinner />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onBeforeUnmount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Image from '@tiptap/extension-image'
import Link from '@tiptap/extension-link'
import TextAlign from '@tiptap/extension-text-align'
import Underline from '@tiptap/extension-underline'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import ToggleSwitch from 'primevue/toggleswitch'
import ProgressSpinner from 'primevue/progressspinner'
import api from '@/services/api'

const route = useRoute()
const router = useRouter()
const toast = useToast()

const isNew = computed(() => route.params.id === 'novy')
const loading = ref(false)
const saving = ref(false)
const articleId = ref<string | null>(null)
const imageUrl = ref<string | null>(null)
const selectedImageFile = ref<File | null>(null)
const coverImageInput = ref<HTMLInputElement | null>(null)
const inlineImageInput = ref<HTMLInputElement | null>(null)
const uploadingInlineImage = ref(false)
const form = ref({ title: '', top: false })
const htmlMode = ref(false)
const rawHtml = ref('')

function toggleHtmlMode() {
  if (!htmlMode.value) {
    rawHtml.value = editor.value?.getHTML() ?? ''
  } else {
    editor.value?.commands.setContent(rawHtml.value)
  }
  htmlMode.value = !htmlMode.value
}

const editor = useEditor({
  extensions: [
    StarterKit,
    Underline,
    Image,
    Link.configure({ openOnClick: false }),
    TextAlign.configure({ types: ['heading', 'paragraph'] }),
  ],
  editorProps: {
    attributes: { class: 'tiptap-content' },
  },
})

onMounted(async () => {
  if (isNew.value) return
  loading.value = true
  const { data } = await api.get(`/articles/${route.params.id}`)
  articleId.value = data.id
  form.value = { title: data.title ?? '', top: data.top }
  editor.value?.commands.setContent(data.text ?? '')
  if (data.guidImage && data.guidImage !== '00000000-0000-0000-0000-000000000000') {
    imageUrl.value = `/File?id=${data.guidImage}`
  }
  loading.value = false
})

onBeforeUnmount(() => editor.value?.destroy())

async function save() {
  if (!form.value.title.trim()) {
    toast.add({ severity: 'warn', summary: 'Chybí nadpis', life: 3000 })
    return
  }
  if (!selectedImageFile.value && !imageUrl.value) {
    toast.add({ severity: 'warn', summary: 'Obrázek je povinný', life: 3000 })
    return
  }
  saving.value = true
  try {
    const formData = new FormData()
    formData.append('title', form.value.title)
    formData.append('text', editor.value?.getHTML() ?? '')
    formData.append('top', form.value.top.toString())
    
    if (selectedImageFile.value) {
      formData.append('image', selectedImageFile.value)
    }
    
    if (isNew.value) {
      const { data: id } = await api.post<string>('/articles/create', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      })
      articleId.value = id
      selectedImageFile.value = null
      toast.add({ severity: 'success', summary: 'Článek vytvořen', life: 2000 })
      router.replace(`/clanky/${id}`)
    } else {
      await api.put(`/articles/image/${articleId.value}`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      })
      selectedImageFile.value = null
      toast.add({ severity: 'success', summary: 'Uloženo', life: 2000 })
    }
  } finally {
    saving.value = false
  }
}

function triggerCoverUpload() {
  coverImageInput.value?.click()
}

function onCoverImageChanged(event: Event) {
  const input = event.target as HTMLInputElement
  const file = input.files?.[0] ?? null
  selectedImageFile.value = file
  if (file) {
    const reader = new FileReader()
    reader.onload = (e) => {
      imageUrl.value = (e.target?.result as string) ?? null
    }
    reader.readAsDataURL(file)
  }
  input.value = ''
}

function triggerInlineImageUpload() {
  inlineImageInput.value?.click()
}

async function onInlineImageSelected(event: Event) {
  const input = event.target as HTMLInputElement
  const file = input.files?.[0]
  if (!file) return

  uploadingInlineImage.value = true
  try {
    const formData = new FormData()
    formData.append('file', file)
    const { data } = await api.post<{ guid: string; url: string }>('/articles/inline-image', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })

    editor.value?.chain().focus().setImage({ src: data.url }).run()
    toast.add({ severity: 'success', summary: 'Obrázek vložen do textu', life: 2000 })
  } finally {
    uploadingInlineImage.value = false
    input.value = ''
  }
}

function setLink() {
  const prev = editor.value?.getAttributes('link').href ?? ''
  const url = window.prompt('URL odkazu', prev)
  if (url === null) return
  if (url === '') {
    editor.value?.chain().focus().unsetLink().run()
  } else {
    editor.value?.chain().focus().setLink({ href: url }).run()
  }
}
</script>

<style scoped>
.editor-page {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.editor-header {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem 1.5rem;
  background: white;
  border-bottom: 1px solid #e5e7eb;
  flex-shrink: 0;
}

.editor-header h2 {
  font-size: 1.1rem;
  font-weight: 700;
  flex: 1;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.header-actions label {
  font-size: 0.875rem;
  color: #555;
}

.editor-body {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  padding: 1rem 1.5rem;
  gap: 0.75rem;
}

.meta-row .title-input {
  width: 100%;
  font-size: 1.1rem;
}

.image-row {
  display: flex;
  align-items: center;
  gap: 12px;
}

.image-thumb {
  width: 80px;
  height: 52px;
  border-radius: 8px;
  border: 1.5px dashed var(--border);
  background: #f9fafb;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 3px;
  font-size: 0.625rem;
  color: var(--text-muted);
  flex-shrink: 0;
  cursor: pointer;
  overflow: hidden;
  transition: border-color 0.15s;
}

.image-thumb:hover {
  border-color: var(--green);
}

.image-thumb.has-image {
  border-style: solid;
  border-color: var(--border);
}

.image-thumb img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.image-info {
  flex: 1;
}

.image-label {
  font-size: 0.78rem;
  color: var(--text-secondary);
  font-weight: 500;
}

.image-hint {
  font-size: 0.72rem;
  color: var(--text-muted);
}

/* Toolbar */
.toolbar {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
  padding: 0.5rem;
  background: #f9fafb;
  border: 1px solid #e5e7eb;
  border-bottom: none;
  border-radius: 8px 8px 0 0;
}

.toolbar-group {
  display: flex;
  gap: 0.125rem;
  padding-right: 0.5rem;
  border-right: 1px solid #e5e7eb;
}

.toolbar-group:last-child {
  border-right: none;
  padding-right: 0;
}

.toolbar button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 30px;
  border: none;
  background: transparent;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.8rem;
  font-weight: 600;
  color: #374151;
  transition: background 0.1s;
}

.toolbar button:hover:not(:disabled) { background: #e5e7eb; }
.toolbar button.active { background: #93C11F; color: white; }
.toolbar button:disabled { opacity: 0.35; cursor: default; }

/* Editor */
.tiptap-wrapper {
  flex: 1;
  overflow-y: auto;
  border: 1px solid #e5e7eb;
  border-radius: 0 0 8px 8px;
  background: white;
}

.tiptap-wrapper :deep(.tiptap-content) {
  min-height: 100%;
  padding: 1.25rem 1.5rem;
  outline: none;
  font-size: 0.95rem;
  line-height: 1.7;
}

.tiptap-wrapper :deep(h2) { font-size: 1.4rem; font-weight: 700; margin: 1rem 0 0.5rem; }
.tiptap-wrapper :deep(h3) { font-size: 1.15rem; font-weight: 700; margin: 0.75rem 0 0.4rem; }
.tiptap-wrapper :deep(p) { margin: 0 0 0.75rem; }
.tiptap-wrapper :deep(ul), .tiptap-wrapper :deep(ol) { padding-left: 1.5rem; margin: 0 0 0.75rem; }
.tiptap-wrapper :deep(blockquote) { border-left: 3px solid #93C11F; margin: 0 0 0.75rem; padding-left: 1rem; color: #6b7280; }
.tiptap-wrapper :deep(a) { color: #93C11F; text-decoration: underline; }
.tiptap-wrapper :deep(img) { max-width: 100%; border-radius: 4px; }
.tiptap-wrapper :deep(.ProseMirror-focused) { outline: none; }
.tiptap-wrapper :deep(p.is-editor-empty:first-child::before) {
  content: attr(data-placeholder);
  color: #9ca3af;
  float: left;
  pointer-events: none;
  height: 0;
}

.loading-state {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
}

.html-textarea {
  flex: 1;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 0 0 8px 8px;
  resize: none;
  font-family: 'Menlo', 'Consolas', monospace;
  font-size: 0.8rem;
  line-height: 1.6;
  background: #1e1e1e;
  color: #d4d4d4;
  outline: none;
}
</style>
