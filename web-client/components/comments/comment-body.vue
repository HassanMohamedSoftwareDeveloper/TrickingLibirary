<template>
  <div class="my-1">
    <div>
      <p class="mb-1" v-html="comment.htmlContent"></p>
      <v-btn small text v-if="!replying" @click="replying = true">Reply</v-btn>
      <v-btn
        small text
        v-if="$listeners['load-replies']"
        @click="$emit('load-replies')"
        >Load Replies</v-btn
      >
    </div>
    <comment-input
      v-if="replying"
      @send="(c) => $emit('send', c)"
      @cancel="replying = false"
    />
  </div>
</template>

<script>
import commentInput from "./comment-input.vue";
export default {
  components: { commentInput },
  name: "comment-body",
  props: {
    comment: {
      required: true,
      type: Object,
    },
  },
  data: () => ({
    replying: false,
  }),
};
</script>
