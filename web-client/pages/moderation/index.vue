<template>
  <div>
    <v-btn
      :to="`/moderation/${i.id}/${i.type}/${i.target}`"
      v-for="i in items"
      :key="i.id"
      >{{ i.target }}</v-btn
    >
  </div>
</template>

<script>
export default {
  data: () => ({
    items: [],
  }),
  async fetch() {
    this.items = await this.$axios.$get("/api/ModerationItem");
  },
};
</script>
