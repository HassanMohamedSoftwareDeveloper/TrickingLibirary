<template>
  <v-card class="mb-3">
    <user-header
      :username="submission.user.username"
      :imageUrl="submission.user.image"
    />
    <video-player :video="submission.video" :thumb="submission.thumb" />
    <v-card-text>{{ submission.description }}</v-card-text>
  </v-card>
</template>

<script>
import videoPlayer from "@/components/video-player.vue";
import UserHeader from "@/components/user-header.vue";

export default {
  name: "submission",
  props: {
    submission: {
      type: Object,
      required: true,
    },
  },
  components: { videoPlayer, UserHeader },
};
</script>
