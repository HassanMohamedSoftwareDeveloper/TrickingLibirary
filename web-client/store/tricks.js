const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: []
});

export const state = initState;
export const getters = {
  trickById: state => id => state.tricks.find(x => x.id === id),
  categoryById: state => id => state.categories.find(x => x.id === id),
  difficultyById: state => id => state.difficulties.find(x => x.id === id),
  trickItems: state => state.tricks.map(x => ({
    text: x.name,
    value: x.id
  })),
  categoryItems: state => state.categories.map(x => ({
    text: x.name,
    value: x.id
  })),
  difficultyItems: state => state.difficulties.map(x => ({
    text: x.name,
    value: x.id
  })),
};

export const mutations = {
  setTricks(state, { tricks, categories, difficulties }) {
    state.tricks = tricks;
    state.categories = categories;
    state.difficulties = difficulties;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get("/api/trick");
    const categories = await this.$axios.$get("/api/categories");
    const difficulties = await this.$axios.$get("/api/difficulties");
    console.log(tricks, categories, difficulties);
    commit("setTricks", { tricks, categories, difficulties });
  },
  createTrick({ state, commit, dispatch }, { form }) {
    return this.$axios.$post("/api/trick", form);
  },
};
