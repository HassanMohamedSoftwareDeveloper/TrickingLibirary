export { default as HeaderMenu } from '../..\\components\\header-menu.vue'
export { default as ItemContentLayout } from '../..\\components\\item-content-layout.vue'
export { default as TrickList } from '../..\\components\\trick-list.vue'
export { default as VideoPlayer } from '../..\\components\\video-player.vue'
export { default as ContentCreationCategoryForm } from '../..\\components\\content-creation\\category-form.vue'
export { default as ContentCreationDialog } from '../..\\components\\content-creation\\content-creation-dialog.vue'
export { default as ContentCreationDifficultyForm } from '../..\\components\\content-creation\\difficulty-form.vue'
export { default as ContentCreationSubmissionSteps } from '../..\\components\\content-creation\\submission-steps.vue'
export { default as ContentCreationTrickSteps } from '../..\\components\\content-creation\\trick-steps.vue'
export { default as ContentCreationShared } from '../..\\components\\content-creation\\_shared.js'
export { default as CommentsCommentBody } from '../..\\components\\comments\\comment-body.vue'
export { default as CommentsCommentInput } from '../..\\components\\comments\\comment-input.vue'
export { default as CommentsCommentSection } from '../..\\components\\comments\\comment-section.vue'
export { default as CommentsComment } from '../..\\components\\comments\\comment.vue'

// nuxt/nuxt.js#8607
function wrapFunctional(options) {
  if (!options || !options.functional) {
    return options
  }

  const propKeys = Array.isArray(options.props) ? options.props : Object.keys(options.props || {})

  return {
    render(h) {
      const attrs = {}
      const props = {}

      for (const key in this.$attrs) {
        if (propKeys.includes(key)) {
          props[key] = this.$attrs[key]
        } else {
          attrs[key] = this.$attrs[key]
        }
      }

      return h(options, {
        on: this.$listeners,
        attrs,
        props,
        scopedSlots: this.$scopedSlots,
      }, this.$slots.default)
    }
  }
}
