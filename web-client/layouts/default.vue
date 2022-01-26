<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Libirary</v-toolbar-title>
      <v-spacer></v-spacer>
       <!-- <v-btn  depressed @click="toggleActivity"> Upload </v-btn> -->
    </v-app-bar>
    <content-creation-dialog></content-creation-dialog>
    <v-main>
      <Nuxt />
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from '../components/content-creation/content-creation-dialog.vue';
export default {
  components: {  ContentCreationDialog },
};
</script>
