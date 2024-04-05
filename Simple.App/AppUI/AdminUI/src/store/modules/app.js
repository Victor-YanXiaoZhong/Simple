
const state = {
  sidebar: {
    opened: true,
    withoutAnimation: false
  },
  device: 'desktop',
  menus:[],
  auths:[]
}

const mutations = {
  TOGGLE_SIDEBAR: state => {
    state.sidebar.opened = !state.sidebar.opened
    state.sidebar.withoutAnimation = false
  },
  CLOSE_SIDEBAR: (state, withoutAnimation) => {
    state.sidebar.opened = false
    state.sidebar.withoutAnimation = withoutAnimation
  },
  TOGGLE_DEVICE: (state, device) => {
    state.device = device
  },
  SET_MENUS: (state, menu) => {
    state.menus = menu
  },
  SET_AUTHS: (state, auth) => {
    state.auths = auth
  }
}

const actions = {
  toggleSideBar({ commit }) {
    commit('TOGGLE_SIDEBAR')
  },
  closeSideBar({ commit }, { withoutAnimation }) {
    commit('CLOSE_SIDEBAR', withoutAnimation)
  },
  toggleDevice({ commit }, device) {
    commit('TOGGLE_DEVICE', device)
  },
  setMenus({ commit }, menus) {
    commit('SET_MENUS', menus)
  },
  setAuths({ commit }, auths) {
    commit('SET_AUTHS', auths)

  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
