const initState = () => ({
  dictionary: {
    tricks: null,
    categories: null,
    difficulties: null
  },
  lists: {
    tricks: [],
    categories: [],
    difficulties: []
  },
});

export const state = initState;

const setEntities = (state, type, data) => {
  state.dictionary[type] = {};
  data.forEach(x => {
    state.lists[type].push(x);
    state.dictionary[type][x.id] = x;
    if(x.slug){
      state.dictionary[type][x.slug] = x;
    }
  });
}

export const mutations = {
  setTricks(state, { tricks }) {
    setEntities(state, "tricks", tricks);
  },
  setCategories(state, { categories }) {
    setEntities(state, "categories", categories);
  },
  setDifficulties(state, { difficulties }) {
    setEntities(state, "difficulties", difficulties);
  },

  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  fetchTricks({ commit }) {
    return Promise.all([
      this.$axios.$get("/api/trick").then(tricks => commit("setTricks", { tricks })),
      this.$axios.$get("/api/categories").then(categories => commit("setCategories", { categories })),
      this.$axios.$get("/api/difficulties").then(difficulties => commit("setDifficulties", { difficulties })),
    ]);
  },
  createTrick({ state, commit, dispatch }, { form }) {
    return this.$axios.$post("/api/trick", form);
  },
  updateTrick({ state, commit, dispatch }, { form }) {
    return this.$axios.$put("/api/trick", form);
  },
};
