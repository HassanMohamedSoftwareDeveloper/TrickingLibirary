<template>
  <v-app dark>
    <v-app-bar app dense>
      <nuxt-link
        class="text-h5 text--primary"
        style="text-decoration: none"
        to="/"
        >Tricking Libirary</nuxt-link
      >
      <v-spacer></v-spacer>
      <v-btn class="mx-1" v-if="moderator" depressed to="/moderation"
        >Moderation</v-btn
      >

      <v-skeleton-loader
        class="mx-1"
        :loading="loading"
        transition="fade-transition"
        type="button"
      >
        <HeaderMenu></HeaderMenu>
      </v-skeleton-loader>
      <v-skeleton-loader
        class="mx-1"
        :loading="loading"
        transition="fade-transition"
        type="button"
      >
        <v-btn depressed outlined v-if="authenticated">
          <v-icon left>mdi-account-circle</v-icon>Profile
        </v-btn>
        <v-btn @click="$auth.signinRedirect()" depressed outlined v-else>
          <v-icon left>mdi-account-circle-outline</v-icon>Sign In
        </v-btn>
      </v-skeleton-loader>
      <v-btn @click="$auth.signoutRedirect()" depressed v-if="authenticated">
        Sign Out
      </v-btn>
    </v-app-bar>
    <v-skeleton-loader
      class="mx-1"
      :loading="loading"
      transition="fade-transition"
      type="button"
    >
      <content-creation-dialog></content-creation-dialog>
    </v-skeleton-loader>
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import ContentCreationDialog from "../components/content-creation/content-creation-dialog.vue";
import HeaderMenu from "../components/header-menu.vue";
import { mapState, mapGetters } from "vuex";
export default {
  components: { ContentCreationDialog, HeaderMenu },
  computed: {
    ...mapState("auth", ["loading"]),
    ...mapGetters("auth", ["authenticated", "moderator"]),
  },
};
</script>
