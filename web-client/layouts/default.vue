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
        <v-menu offset-y v-if="authenticated">
          <template v-slot:activator="{ on, attrs }">
            <v-btn icon v-bind="attrs" v-on="on" depressed>
              <v-avatar size="36">
                <img
                  v-if="profile.image"
                  :src="profile.image"
                  alt="Profile Image"
                />
                <v-icon v-else>mdi-account-circle</v-icon>
              </v-avatar>
            </v-btn>
          </template>
          <v-list>
            <v-list-item @click="$router.push('/profile')">
              <v-list-item-title>
                <v-icon left>mdi-account-circle</v-icon>Profile
              </v-list-item-title>
            </v-list-item>
            <v-list-item @click="$auth.signoutRedirect()">
              <v-list-item-title>
                <v-icon left>mdi-logout</v-icon>Logout
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
        <v-btn @click="login" depressed outlined v-else>
          <v-icon left>mdi-account-circle-outline</v-icon>Login
        </v-btn>
      </v-skeleton-loader>
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
import { mapState, mapGetters,mapActions } from "vuex";
export default {
  components: {
    ContentCreationDialog,
    HeaderMenu,
  },
  computed: {
    ...mapState("auth", ["loading", "profile"]),
    ...mapGetters("auth", ["authenticated", "moderator"]),
  },
  methods:mapActions("auth",["login"]),
};
</script>
