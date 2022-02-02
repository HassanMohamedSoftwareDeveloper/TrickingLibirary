<template>
  <div>
    <comment-input label="Comment" @send="(c) => $emit('send', c)" />
    <v-divider class="my-5" />

    <comment v-for="comment in comments" :comment="comment" :key="comment.id" />
  </div>
</template>

<script>
import commentInput from "./comment-input.vue";
import Comment from "./comment.vue";
export default {
  name: "comment-section",
  components: { commentInput, Comment },
  props: {
    comments: {
      required: true,
      type: Array,
    },
  },
};
</script>
