<template>
  <v-dialog :value="active" persistent>
    <div v-if="component">
      <component :is="component"></component>
    </div>
    <div class="d-flex justify-center my-4">
      <v-btn @click="reset">Close</v-btn>
    </div>
  </v-dialog>
</template>

<script>
import { mapState, mapMutations } from "vuex";
export default {
  name: "content-creation-dialog",
  computed: mapState("video-upload", ["active", "component"]),
  methods: mapMutations("video-upload", ["reset"]),
};
</script>
