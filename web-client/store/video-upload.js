const initState = () => ({
  uploadPromise: null,
  active: false,
  type: "",
  step:1
});


export const state = initState;

export const mutations = {
  toggleActivity(state) {
    state.active = !state.status
    if (!state.active) {
      Object.assign(state, initState());
    }
  },
  setType(state, { type }) {
    console.log(type)
    state.type = type
    state.step++
  },
  setTask(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise;
    state.step++
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {

  startVideoUpload({ commit, dispatch }, { form }) {
    const uploadPromise = this.$axios.$post("/api/videos", form);
    commit('setTask', { uploadPromise })
  },
  async createTrick({ commit, dispatch }, { trick }) {
    await this.$axios.post("/api/trick", trick);
    await dispatch('tricks/fetchTricks');
  },
};
