import { createApp } from 'vue'
import '@/assets/main.css'
import pinia from '@/store/index'

import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import App from './App.vue'
import router from './router'

const app = createApp(App)
app.use(pinia)
app.use(router)
// 挂载到app实例上
app.use(ElementPlus,{ size:'small' })
app.mount('#app')
