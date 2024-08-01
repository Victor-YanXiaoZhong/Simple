<template>
    <el-header class="navbar">
      <div class="left">
        
      </div>
      <div class="right">
        <el-space wrap>
          <!-- <el-icon :title="'点击刷新当前页面'" @click="toRefresh" style="font-size: 20px;cursor: pointer;"><refresh/></el-icon> -->
          <el-icon :title="'点击切换全屏'" @click="toFullScreen" style="font-size: 20px;cursor: pointer;"><full-screen /></el-icon>
          <el-dropdown class="avatar-container" trigger="click" @command="handleCommand">
            <div style="display: flex;align-items: center;"> 
              <el-avatar :size="20" :src="avatar" /><span style="cursor: pointer;">{{ appState.user.Name }}</span>
            </div>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item divided command="personal">个人中心</el-dropdown-item>
                <el-dropdown-item divided command="logout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </el-space>
      </div>
    </el-header>
  </template>
  <script setup lang="ts">
  import { ref, computed, watch } from 'vue';
  import { useRoute, useRouter } from 'vue-router'
  import {FullScreen,Refresh} from '@element-plus/icons-vue'
  import {appState, appfunctions} from "@/store/glable.js"; //引入仓库'

  const defaultAvatar = ref('/default_avatar.png')

  const router = useRouter()
  
  const avatar = computed(() => {
    return (appState.value.user&&appState.value.user.avatar != null) || defaultAvatar.value
  })

  const handleCommand = (command:string) => {
    if (command === 'logout') {
        appfunctions.logout()
        router.push({path: '/login'})
    }
    if (command === 'personal') {
        router.push({path: '/user/profile'})
    }
  } 

  // const toRefresh = () => {
  //   let path = router.currentRoute.value.path
  //   debugger
  //   router.push({ path, query: { refresh: new Date().getTime() } });
  // }

  const toFullScreen = () => {
    const doc = window.document
    const docEl = doc.documentElement
    const requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen
    const cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen
    if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement) {
      requestFullScreen.call(docEl)
    } else {
      cancelFullScreen.call(doc)
    }
  }
  </script>
  <style lang="scss" scoped>
  .navbar {
    display: flex;
    flex: 1;
    height: 30px;
    line-height: 30px;
    align-items: center;
    justify-content: space-between;
    overflow: hidden;
    position: relative;
    background: #fff;
    .collapse {
      cursor: pointer;
      font-size: 24px;
    }
    .left,
    .right {
      display: flex;
      align-items: center;
    }
    .avatar-container {
      .el-avatar {
        cursor: pointer;
      }
    }
    .el-breadcrumb {
      margin-left: 8px;
    }
    .el-avatar{background: none;}
  }
  </style>