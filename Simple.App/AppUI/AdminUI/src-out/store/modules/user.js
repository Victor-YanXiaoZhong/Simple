const getDefaultState = () => {
  return {
    info: {
      Name: '',
      Account: '',
      Id: 0,
      Avatar: '',
      Phone: '',
      SupperAdmin: false,
      SysOrgnizationId: 0,
      Token: '',
      LogTime: '',
      AdminOrg: false,
    },
    device: 'desktop',
  }
}

const state = getDefaultState()

const mutations = {
  SET_INFO: (state, info) => {
    state.info = info
  },
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
}

const actions = {
  setUserinfo({ commit }, info) {
    commit('SET_INFO', info)
  },
  userLogOut({ commit }) {
    commit('RESET_STATE')
  },
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
}
