import Vue from 'vue'
import Router from 'vue-router'
import { normalizeURL, decode } from 'ufo'
import { interopDefault } from './utils'
import scrollBehavior from './router.scrollBehavior.js'

const _58831c8d = () => interopDefault(import('..\\pages\\moderation\\index.vue' /* webpackChunkName: "pages/moderation/index" */))
const _3a052fb8 = () => interopDefault(import('..\\pages\\profile\\index.vue' /* webpackChunkName: "pages/profile/index" */))
const _9dee7300 = () => interopDefault(import('..\\pages\\category\\_category.vue' /* webpackChunkName: "pages/category/_category" */))
const _0a2820e0 = () => interopDefault(import('..\\pages\\difficulty\\_difficulty.vue' /* webpackChunkName: "pages/difficulty/_difficulty" */))
const _e5ae6eda = () => interopDefault(import('..\\pages\\moderation\\_modId.vue' /* webpackChunkName: "pages/moderation/_modId" */))
const _4ddd6ffa = () => interopDefault(import('..\\pages\\tricks\\_trick.vue' /* webpackChunkName: "pages/tricks/_trick" */))
const _0e557673 = () => interopDefault(import('..\\pages\\index.vue' /* webpackChunkName: "pages/index" */))

const emptyFn = () => {}

Vue.use(Router)

export const routerOptions = {
  mode: 'history',
  base: '/',
  linkActiveClass: 'nuxt-link-active',
  linkExactActiveClass: 'nuxt-link-exact-active',
  scrollBehavior,

  routes: [{
    path: "/moderation",
    component: _58831c8d,
    name: "moderation"
  }, {
    path: "/profile",
    component: _3a052fb8,
    name: "profile"
  }, {
    path: "/category/:category?",
    component: _9dee7300,
    name: "category-category"
  }, {
    path: "/difficulty/:difficulty?",
    component: _0a2820e0,
    name: "difficulty-difficulty"
  }, {
    path: "/moderation/:modId",
    component: _e5ae6eda,
    name: "moderation-modId"
  }, {
    path: "/tricks/:trick?",
    component: _4ddd6ffa,
    name: "tricks-trick"
  }, {
    path: "/",
    component: _0e557673,
    name: "index"
  }],

  fallback: false
}

export function createRouter (ssrContext, config) {
  const base = (config._app && config._app.basePath) || routerOptions.base
  const router = new Router({ ...routerOptions, base  })

  // TODO: remove in Nuxt 3
  const originalPush = router.push
  router.push = function push (location, onComplete = emptyFn, onAbort) {
    return originalPush.call(this, location, onComplete, onAbort)
  }

  const resolve = router.resolve.bind(router)
  router.resolve = (to, current, append) => {
    if (typeof to === 'string') {
      to = normalizeURL(to)
    }
    return resolve(to, current, append)
  }

  return router
}
