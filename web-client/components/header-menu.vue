<template>
  <v-menu offset-y>
    <template v-slot:activator="{ on, attrs }">
      <v-btn
        depressed
        v-bind="attrs"
        v-on="on"
        >Create</v-btn
      >
    </template>
    <v-list>
      <v-list-item
        v-for="(item, index) in menuItems"
        :key="`ccd-menu-${index}`"
        @click="activate({ component: item.component })"
      >
        <v-list-item-title>{{ item.title }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>

<script>
import { mapMutations } from "vuex";
import TrickSteps from "./content-creation/trick-steps.vue";
import SubmissionSteps from "./content-creation/submission-steps.vue";
export default {
  components: {
    TrickSteps,
    SubmissionSteps,
  },
  name: "content-creation-dialog",
  computed: {
    menuItems() {
      return [
        { component: TrickSteps, title: "Create Trick" },
        { component: SubmissionSteps, title: "Submission" },
      ];
    },
  },
  methods: mapMutations("video-upload", ["activate"]),
};
</script>
