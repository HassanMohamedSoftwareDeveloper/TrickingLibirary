import Vue from 'vue'
import Router from 'vue-router'
import { normalizeURL, decode } from 'ufo'
import { interopDefault } from './utils'
import scrollBehavior from './router.scrollBehavior.js'

const _487f2e57 = () => interopDefault(import('..\\pages\\category\\_category.vue' /* webpackChunkName: "pages/category/_category" */))
const _a42e7592 = () => interopDefault(import('..\\pages\\difficulty\\_difficulty.vue' /* webpackChunkName: "pages/difficulty/_difficulty" */))
const _4d7115cc = () => interopDefault(import('..\\pages\\tricks\\_trick.vue' /* webpackChunkName: "pages/tricks/_trick" */))
const _47c6528a = () => interopDefault(import('..\\pages\\index.vue' /* webpackChunkName: "pages/index" */))

const emptyFn = () => {}

Vue.use(Router)

export const routerOptions = {
  mode: 'history',
  base: '/',
  linkActiveClass: 'nuxt-link-active',
  linkExactActiveClass: 'nuxt-link-exact-active',
  scrollBehavior,

  routes: [{
    path: "/category/:category?",
    component: _487f2e57,
    name: "category-category"
  }, {
    path: "/difficulty/:difficulty?",
    component: _a42e7592,
    name: "difficulty-difficulty"
  }, {
    path: "/tricks/:trick?",
    component: _4d7115cc,
    name: "tricks-trick"
  }, {
    path: "/",
    component: _47c6528a,
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
