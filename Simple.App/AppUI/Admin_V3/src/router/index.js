
import AppLayout from "@/layout/AppLayout.vue";
import { createRouter, createWebHistory } from 'vue-router'
import generaroutes from "@/utils/autorouter";

// 生成动态路由规则
const autoGenerator = generaroutes;
const constRoutes = [
  {
    path: "/",
    name: "首页",
    redirect: "/home",
  },
  {
    path: "/login",
    name: "登录",
    component: () => import("@/constviews/Login.vue"),
  },
  {
    path: "/404",
    name: "404",
    component: () => import("@/constviews/404.vue"),
  },
  { path: "/:catchAll(.*)", redirect: "/404", hidden: true },
];

const routes = autoGenerator.concat(...constRoutes);
console.log(routes);
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
