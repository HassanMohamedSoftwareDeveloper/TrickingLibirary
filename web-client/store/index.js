const initState = () => ({})

export const state = initState

export const mutations = {
  setMessage(state, message) {
    state.message = message
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  nuxtServerInit({ dispatch }) {
    return dispatch("tricks/fetchTricks");
  }
}
