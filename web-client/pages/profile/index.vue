<template>
  <v-card>
    <v-card-title>
      <v-avatar>
        <v-icon>mdi-account</v-icon>
      </v-avatar>
      Test User
    </v-card-title>
    <v-card-text>
      <div v-if="submissions">
        <v-card class="mb-3" v-for="s in submissions" :key="`${s.id}`">
          <video-player :video="s.video" :key="`v-${s.id}`"/>
          <v-card-text>{{ s.description }}</v-card-text>
        </v-card>
      </div>
    </v-card-text>
  </v-card>
</template>
<script>
import videoPlayer from "../../components/video-player.vue";
export default {
  components: { videoPlayer },
  data: () => ({
    submissions: [],
  }),
  async mounted() {
    return this.$store.dispatch("auth/_watchUserLoaded", async () => {
      const profile = this.$store.state.auth.profile;
      this.submissions = await this.$axios.$get(
        `/api/user/${profile.id}/submissions`
      );
    });
  },
};
</script>
