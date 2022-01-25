const initState = () => ({
  tricks: []
});

export const state = initState;

export const mutations = {
  setTricks(state, { tricks }) {
    state.tricks = tricks;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async fetchTricks({ commit }) {//fetchMessage
    const tricks = await this.$axios.$get("/api/trick");
    commit("setTricks", { tricks });
  },

};
