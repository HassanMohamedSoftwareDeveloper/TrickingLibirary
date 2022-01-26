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
  async nuxtServerInit({ dispatch }) {
    dispatch("tricks/fetchTricks");
  }
}
