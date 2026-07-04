import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import { definePreset } from '@primeuix/themes'
import Aura from '@primeuix/themes/aura'
import ToastService from 'primevue/toastservice'
import ConfirmationService from 'primevue/confirmationservice'
import Tooltip from 'primevue/tooltip'
import 'primeicons/primeicons.css'

import './style.css'
import App from './App.vue'
import router from './router'

const MinersPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50:  '#f4fadc',
      100: '#e6f5b3',
      200: '#d4ec80',
      300: '#bfe04c',
      400: '#aad424',
      500: '#93C11F',
      600: '#7DAA1A',
      700: '#668f14',
      800: '#4f730f',
      900: '#375809',
      950: '#1e3204',
    },
  },
  components: {
    datatable: {
      root: {
        borderColor: '#e8eaed',
      },
      headerCell: {
        background: '#fff',
        hoverBackground: '#fff',
        borderColor: '#e8eaed',
        color: '#6b7280',
        padding: '11px 16px',
        sm: { padding: '11px 16px' },
      },
      columnTitle: {
        fontWeight: '700',
      },
      row: {
        background: '#fff',
        hoverBackground: '#fafafa',
        color: '#1a1d23',
        stripedBackground: '#fafbfc',
      },
      bodyCell: {
        borderColor: '#f3f4f6',
        padding: '11px 16px',
        sm: { padding: '11px 16px' },
      },
      paginatorBottom: {
        borderColor: '#e8eaed',
        borderWidth: '1.5px 0 0 0',
      },
    },
    paginator: {
      root: {
        background: '#fafbfc',
        padding: '8px 16px',
        borderRadius: '0',
      },
      navButton: {
        background: '#fff',
        hoverBackground: '#f3f4f6',
        selectedBackground: '#93C11F',
        color: '#374151',
        hoverColor: '#111318',
        selectedColor: '#fff',
        width: '28px',
        height: '28px',
        borderRadius: '7px',
      },
      currentPageReport: {
        color: '#9ca3af',
      },
    },
  },
})

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(PrimeVue, {
  theme: {
    preset: MinersPreset,
    options: {
      darkModeSelector: '.dark',
    },
  },
})
app.use(ToastService)
app.use(ConfirmationService)
app.directive('tooltip', Tooltip)

app.mount('#app')
