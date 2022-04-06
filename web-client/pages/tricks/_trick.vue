<template>
  <item-content-layout>
    <template v-slot:content>
      <Submission :submission="s"
                  v-for="s in submissions"
                  :key="`submission-${s.id}`" />
    </template>
    <template v-slot:item="{ close }">
      <trick-info-card :trick="trick" :close="close" />
    </template>
  </item-content-layout>
</template>

<script>
  import { mapState, mapMutations } from "vuex";
  
  import ItemContentLayout from "../../components/item-content-layout.vue";
  import Submission from "../../components/submission.vue";
  import TrickInfoCard from "../../components/trick-info-card.vue";
  export default {
    components: { TrickInfoCard, ItemContentLayout, Submission },
    data: () => ({
      trick: null,
    }),

    computed: {
      ...mapState("submissions", ["submissions"]),
      ...mapState("tricks", ["dictionary"]),
    },
    async fetch() {
      const trickSlug = this.$route.params.trick;
      this.trick = this.dictionary.tricks[trickSlug];

      await this.$store.dispatch(
        "submissions/fetchSubmissionsForTrick",
        { trickId: trickSlug },
        { root: true }
      );
    },
    head() {
      if (!this.trick) return {};
      return {
        title: this.trick.name,
        meta: [
          {
            hid: "description",
            name: "description",
            content: this.trick.description,
          },
        ],
      };
    },
  };
</script>
<style scoped>
  .sticky {
    position: -webkit-sticky;
    position: sticky;
    top: 48px;
  }
</style>
