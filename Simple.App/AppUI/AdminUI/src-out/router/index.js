
import * as VueRouter from 'vue-router'
import AppLayout from '@/layout/AppLayout'

// 递归获取 views 文件夹下的所有.vue文件
const files = require.context('@/views', true, /\.vue$/)
const pages = {}
files.keys().forEach((key) => {
  const pagekey = key.replace(/(\.\/|\.vue)/g, '')
  pages[pagekey] = files(key).default
  if (!pages[pagekey].name) {
    pages[pagekey]['name'] = pagekey.replace(/\//g, '')
  }
})
// 生成路由规则
const autoGenerator = []
Object.keys(pages).forEach((item) => {
  try {
    const path = `/${pages[item].name.replace(/-/g, '/')}`
    autoGenerator.push({
      path: path,
      component: AppLayout,
      // name: pages[item].name.replace(/\//g, ''),
      children: [
        {
          path: '/' + item,
          name: pages[item].name.replace(/\//g, ''),
          // component: () => import(`@/views/${item}`)
          component: pages[item],
        },
      ],
    })
  } catch (error) {
    // eslint-disable-next-line no-debugger
    debugger
    console.error()
  }
})
const constRoutes = [
  {
    path: '/',
    name: '首页',
    redirect: '/home',
  },
  {
    path: '/login',
    name: '登录',
    component: (resolve) => require(['@/constviews/Login.vue'], resolve),
  },
  {
    path: '/404',
    name: '404',
    component: (resolve) => require(['@/constviews/404.vue'], resolve),
  },
  { path: '*', redirect: '/404', hidden: true },
]

const routes = autoGenerator.concat(...constRoutes)
console.log(routes)
const router = VueRouter.createRouter({
  history: VueRouter.createWebHashHistory(),
  routes: routes,
})

export default router
