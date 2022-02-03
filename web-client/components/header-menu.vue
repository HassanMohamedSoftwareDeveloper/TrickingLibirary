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
import CategoryForm from "./content-creation/category-form.vue";
import DifficultyForm from "./content-creation/difficulty-form.vue";
export default {
  components: {
    TrickSteps,
    SubmissionSteps,
  },
  name: "content-creation-dialog",
  computed: {
    menuItems() {
      return [
        { component: TrickSteps, title: "Trick" },
        { component: SubmissionSteps, title: "Submission" },
        { component: DifficultyForm, title: "Difficulty" },
        { component: CategoryForm, title: "Category" },
      ];
    },
  },
  methods: mapMutations("video-upload", ["activate"]),
};
</script>
