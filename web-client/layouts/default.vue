<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Libirary</v-toolbar-title>

      <v-spacer></v-spacer>
      <v-btn depressed @click="toggleActivity"> Upload </v-btn>
    </v-app-bar>
    <video-upload/>
    <v-main>
      <Nuxt />
    </v-main>
  </v-app>
</template>

<script>
import { mapMutations } from "vuex";
import videoUpload from "../components/video-upload.vue";
export default {
  components: { videoUpload },
  methods: mapMutations("video-upload", ["toggleActivity"]),
};
</script>
