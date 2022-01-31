<template>
  <v-dialog :value="active" persistent width="700">
    <div v-if="component">
      <component :is="component"></component>
    </div>
  </v-dialog>
</template>

<script>
import { mapState } from "vuex";
export default {
  name: "content-creation-dialog",
  computed: mapState("video-upload", ["active", "component"]),
};
</script>
