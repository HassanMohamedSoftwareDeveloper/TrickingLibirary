<template>
  <div>
    <div v-if="tricks">
      <div v-for="t in tricks">
        {{ t.trickName }}
        <div>
          <video
            width="400"
            height="400"
            controls
            :src="`http://localhost:52891/api/videos/${t.video}`"
          ></video>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
export default {
  name: "IndexPage",
  computed: mapState("tricks", ["tricks"]),
};
</script>
