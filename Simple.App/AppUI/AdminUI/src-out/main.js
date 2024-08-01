import * as Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import App from './App.vue'
import store from './store'
import router from './router'

window.$vueApp.use(ElementUI, { size: 'small' })

import config from './appconfig.js'
import http from './utils/http.js'
import help from './utils/help.js'

window.$vueApp.config.globalProperties.$config = config
window.$vueApp.config.globalProperties.$http = http
window.$vueApp.config.globalProperties.$help = help

import '@/permission'

window.$vueApp = Vue.createApp(App)
window.$vueApp.mount('#app')
window.$vueApp.config.globalProperties.routerAppend = (path, pathToAppend) => {
  return path + (path.endsWith('/') ? '' : '/') + pathToAppend
}
window.$vueApp.use(store)
window.$vueApp.use(router)
