import * as Vue from 'vue'
import * as Vuex from 'vuex'
import app from './modules/app'
import user from './modules/user'
import VuexPersistence from 'vuex-persist'

const getters = {
  sidebar: (state) => state.app.sidebar,
  device: (state) => state.app.device,
  info: (state) => state.user.info,
  token: (state) => {
    if (state.user.info.Token == '') return state.user.info.Token
    return state.user.info.Token
  },
  menus: (state) => state.app.menus.menuTree,
  auths: (state) => state.app.auths,
}

const vuexLocal = new VuexPersistence({
  storage: window.localStorage,
})

const store = Vuex.createStore({
  strict: false,
  modules: {
    app,
    user,
  },
  getters,
  plugins: [vuexLocal.plugin],
})

export default store
