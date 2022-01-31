<template>
  <v-dialog :value="active" persistent width="700">
    <div v-if="component">
      <component :is="component"></component>
    </div>
    <div class="d-flex justify-center my-4">
      <v-btn @click="cancelUpload">Close</v-btn>
    </div>
  </v-dialog>
</template>

<script>
import { mapState, mapActions } from "vuex";
export default {
  name: "content-creation-dialog",
  computed: mapState("video-upload", ["active", "component"]),
  methods: {
    ...mapActions("video-upload", ["cancelUpload"]),
  },
};
</script>
