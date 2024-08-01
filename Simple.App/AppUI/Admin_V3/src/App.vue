<template>
  <RouterView />
</template>

<script setup>
import { RouterView } from 'vue-router'
// 引入中文包
import zhCn from 'element-plus/dist/locale/zh-cn.mjs'
import {onMounted,watch} from 'vue'
import {appState, appfunctions} from "@/store/glable.js"; //引入仓库'

// 在组件挂载时加载状态
onMounted(() => {
    appfunctions.loadState();
  });
  
// 在状态改变时保存状态
watch(appState, appfunctions.saveState, { deep: true });

</script>

<style lang="scss" scoped>
</style>
