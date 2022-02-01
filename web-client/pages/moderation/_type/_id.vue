<template>
  <div v-if="item">{{item.description}}</div>
</template>

<script>
const endpointResolver = (type) => {
  if (type === "trick") return "trick";
};
export default {
  data: () => ({
    item: null,
  }),
  async fetch() {
    const { type, id } = this.$route.params;
    const endpoint = endpointResolver(type);
    this.item =await this.$axios.$get(`/api/${endpoint}/${id}`);
  },
};
</script>
