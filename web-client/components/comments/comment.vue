<template>
  <div>
    <comment-body :comment="comment" @send="send" @load-replies="loadReplies" />
    <div class="ml-5">
      <comment-body
        v-for="reply in replies"
        :comment="reply"
        @send="send"
        :key="`reply-${reply.id}`"
      />
    </div>
  </div>
</template>

<script>
import commentBody from "./comment-body.vue";
export default {
  components: { commentBody },
  name: "comment",
  props: {
    comment: {
      required: true,
      type: Object,
    },
  },
  data: () => ({
    replies: [],
  }),
  methods: {
    send(content) {
      return this.$axios
        .$post(`/api/comment/${this.comment.id}/replies`, { content })
        .then((reply) => this.replies.push(reply));
    },
    loadReplies() {
     return this.$axios
        .$get(`/api/comment/${this.comment.id}/replies`)
        .then((replies) => (this.replies = replies));
    },
  },
};
</script>
