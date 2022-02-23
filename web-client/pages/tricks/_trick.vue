<template>
  <item-content-layout>
    <template v-slot:content>
      <Submission
        :submission="s"
        v-for="s in submissions"
        :key="`submission-${s.id}`"
      />
    </template>
    <template v-slot:item="{ close }">
      <div>
        <div class="text-h5">
          <span>
            {{ trick.name }}
          </span>
          <v-chip
            small
            class="mb-1 ml-2"
            :to="`/difficulty/${difficulty.id}`"
            >{{ difficulty.name }}</v-chip
          >
        </div>
        <v-divider class="my-1"></v-divider>
        <div class="text-body-2">{{ trick.description }}</div>

        <v-divider class="my-1"></v-divider>
        <div v-for="rd in relatedData" v-if="rd.data.length > 0">
          <div class="text-subtitle-1">{{ rd.title }}</div>
          <v-chip-group>
            <v-chip
              v-for="d in rd.data"
              :key="rd.idFactory(d)"
              small
              :to="rd.routeFactory(d)"
            >
              {{ d.name }}
            </v-chip>
          </v-chip-group>
        </div>
        <v-divider class="mb-2"></v-divider>
        <div>
          <v-btn
            @click="
              edit();
              close();
            "
            outlined
            small
            >Edit</v-btn
          >
        </div>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import { mapState, mapMutations } from "vuex";
import TrickSteps from "../../components/content-creation/trick-steps.vue";
import ItemContentLayout from "../../components/item-content-layout.vue";
import Submission from "../../components/submission.vue";
export default {
  components: { ItemContentLayout, Submission },
  data: () => ({
    trick: null,
    difficulty: null,
  }),
  methods: {
    ...mapMutations("video-upload", ["activate"]),
    edit() {
      this.activate({
        component: TrickSteps,
        edit: true,
        editPayLoad: this.trick,
      });
    },
  },
  computed: {
    ...mapState("submissions", ["submissions"]),
    ...mapState("tricks", ["dictionary"]),

    relatedData() {
      return [
        {
          title: "Categories",
          data: this.trick.categories.map((x) => this.dictionary.categories[x]),
          idFactory: (c) => `category-${c.id}`,
          routeFactory: (c) => `/category/${c.id}`,
        },

        {
          title: "Prerequisites",
          data: this.trick.prerequisites.map((x) => this.dictionary.tricks[x]),
          idFactory: (t) => `trick-${t.id}`,
          routeFactory: (t) => `/tricks/${t.slug}`,
        },

        {
          title: "Progressions",
          data: this.trick.progressions.map((x) => this.dictionary.tricks[x]),
          idFactory: (t) => `trick-${t.id}`,
          routeFactory: (t) => `/tricks/${t.slug}`,
        },
      ];
    },
  },
  async fetch() {
    const trickSlug = this.$route.params.trick;
    this.trick = this.dictionary.tricks[trickSlug];
    this.difficulty = this.dictionary.difficulties[this.trick.difficulty];

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
