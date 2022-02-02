<template>
  <div>
    <v-text-field
      :label="label"
      v-model="content"
      @keydown.ctrl.enter="$emit('send', content)"
    ></v-text-field>
    <v-btn v-if="$listeners['cancel']" @click="$emit('cancel')">Cancel</v-btn>
    <v-btn :disabled="!content" @click="$emit('send', content)">{{
      label
    }}</v-btn>
  </div>
</template>

<script>
export default {
  name: "comment-input",
  props: {
    label: {
      required: false,
      type: String,
      default: "Send",
    },
  },
  data: () => ({
    content: "",
  }),
};
</script>
