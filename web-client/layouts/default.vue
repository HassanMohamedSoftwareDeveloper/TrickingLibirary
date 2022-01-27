<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Libirary</v-toolbar-title>
      <v-spacer></v-spacer>
      <HeaderMenu></HeaderMenu>
    </v-app-bar>
    <content-creation-dialog></content-creation-dialog>
    <v-main>
      <Nuxt />
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from '../components/content-creation/content-creation-dialog.vue';
import HeaderMenu from '../components/header-menu.vue';
export default {
  components: { ContentCreationDialog, HeaderMenu },
};
</script>
