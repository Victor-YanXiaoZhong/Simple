import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import App from './App.vue'
import store from './store'
import router from './router'

Vue.use(ElementUI,{ size: 'small' })
Vue.config.productionTip = false

import config from './appconfig.js'
import http from './utils/http.js'
import help from './utils/help.js'

Vue.prototype.$config = config;
Vue.prototype.$http = http;
Vue.prototype.$help = help;
Vue.config.productionTip = false

import '@/permission'

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
