<template>
  <div >
    <v-menu offset-y  >
      <template v-slot:activator="{ on, attrs }">
        <v-btn style="z-index: 1000;" class="float-right" depressed v-bind="attrs" v-on="on">Create</v-btn>
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

    <v-dialog :value="active" persistent>
      <div v-if="component">
        <component :is="component"></component>
      </div>
      <div  class="d-flex justify-center my-4">
        <v-btn @click="reset">Close</v-btn>
      </div>
    </v-dialog>
  </div>
</template>

<script>
import { mapState, mapMutations } from "vuex";
import TrickSteps from "./trick-steps.vue";
import SubmissionSteps from "./submission-steps.vue";
export default {
  components: {
    TrickSteps,
    SubmissionSteps,
  },
  name: "content-creation-dialog",
  computed: {
    ...mapState("video-upload", ["active", "component"]),
    menuItems() {
      return [
        { component: TrickSteps, title: "Create Trick" },
        { component: SubmissionSteps, title: "Submission" },
      ];
    },
  },
  methods: mapMutations("video-upload", ["reset", "activate"]),
};
</script>
