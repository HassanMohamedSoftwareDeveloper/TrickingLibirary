<template>
  <item-content-layout>
    <template v-slot:content>
      <div v-if="submissions">
        <v-card
          class="mb-3"
          v-for="s in submissions"
          :key="`${trick.slug}-${s.id}`"
        >
          <video-player :video="s.video" :key="`${trick.slug}-${s.id}`" />
          <v-card-text>{{ s.description }}</v-card-text>
        </v-card>
      </div>
    </template>
    <template v-slot:item="{close}">
      <div>
        <div class="text-h5">
          <span>
            {{ trick.name }}
          </span>
          <v-chip
            small
            class="mb-1 ml-2"
            :to="`/difficulty/${difficulty.slug}`"
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
        <v-divider class="my-1"></v-divider>
        <div>
          <v-btn @click="edit();close();" outlined small>Edit</v-btn>
        </div>
      </div>
    </template>
  </item-content-layout>
</template>

<script>
import { mapState, mapGetters, mapMutations } from "vuex";
import TrickSteps from "@/components/content-creation/trick-steps.vue";
import ItemContentLayout from "../../components/item-content-layout.vue";
import videoPlayer from "../../components/video-player.vue";
export default {
  components: { ItemContentLayout, videoPlayer },
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
    ...mapState("tricks", ["categories", "tricks"]),
    ...mapGetters("tricks", ["trickById", "difficultyById"]),

    relatedData() {
      return [
        {
          title: "Categories",
          data: this.categories.filter(
            (x) => this.trick.categories.indexOf(x.slug) >= 0
          ),
          idFactory: (c) => `category-${c.slug}`,
          routeFactory: (c) => `/category/${c.slug}`,
        },

        {
          title: "Prerequisites",
          data: this.tricks.filter(
            (x) => this.trick.prerequisites.indexOf(x.slug) >= 0
          ),
          idFactory: (t) => `trick-${t.slug}`,
          routeFactory: (t) => `/tricks/${t.slug}`,
        },

        {
          title: "Progressions",
          data: this.tricks.filter(
            (x) => this.trick.progressions.indexOf(x.slug) >= 0
          ),
          idFactory: (t) => `trick-${t.slug}`,
          routeFactory: (t) => `/tricks/${t.slug}`,
        },
      ];
    },
  },
  async fetch() {
    const trickId = this.$route.params.trick;
    this.trick = this.trickById(this.$route.params.trick);
    this.difficulty = this.difficultyById(this.trick.difficulty);

    await this.$store.dispatch(
      "submissions/fetchSubmissionsForTrick",
      { trickId },
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
