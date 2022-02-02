<template>
  <div>
    <div v-if="item">{{ item.description }}</div>
    <comment-section :comments="comments" @send="send"/>

    <!--<div class="my-1" v-for="c in comments">
      <span v-html="c.htmlContent"></span>
      <v-btn small @click="replyId = c.id">Reply</v-btn>
      <v-btn small @click="loadReplies(c)">Load Replies</v-btn>
      <div v-for="r in c.replies">
        <span v-html="r.htmlContent"></span>
      </div>
    </div>-->
  </div>
</template>

<script>
import commentSection from "../../../../components/comments/comment-section.vue";
const endpointResolver = (type) => {
  if (type === "trick") return "trick";
};
const commentWithReplies = (comment) => ({
  ...comment,
  replies: [],
});
export default {
  components: { commentSection },
  data: () => ({
    item: null,
    comments: [],
    comment: "",
    replyId: 0,
  }),
  created() {
    const { modId, type, trickId } = this.$route.params;
    const endpoint = endpointResolver(type);
    this.$axios
      .$get(`/api/${endpoint}/${trickId}`)
      .then((item) => (this.item = item));

    this.$axios
      .$get(`/api/ModerationItem/${modId}/comments`)
      .then((comments) => (this.comments = comments.map(commentWithReplies)));
  },
  methods: {
    send(content) {
      const { modId } = this.$route.params;
     return this.$axios
        .$post(`/api/ModerationItem/${modId}/comments`, {content})
        .then((comment) => {
          this.comments.push(comment);
        });

      // if (this.replyId > 0) {
      //   this.$axios
      //     .$post(`/api/comment/${this.replyId}/replies`, {
      //       content: this.comment,
      //     })
      //     .then((reply) => {
      //       this.comments
      //         .find((x) => x.id === this.replyId)
      //         .replies.push(reply);
      //       this.comment = "";
      //     });
      // } else {

      // }
    },


  },
};
</script>
