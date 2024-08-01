<template>
  <el-container :style="{ height: setHeight + 'px' }">
    <el-container>
        <el-menu
          default-active="1"
          class="el-menu-vertical"
          :collapse="!appState.isOpenSidebar"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
          :collapse-transition="false"
        >
        <span class="app-logo" v-if="appState.isOpenSidebar">YOUR LOGO</span>
        <el-sub-menu
            :index="menu.Id + ''"
            v-for="menu in appState.menutree"
            :key="menu.Id + ''"
          >
          <template #title>
              <el-icon><location /></el-icon>
              <span>{{ menu.Name }}</span>
            </template>
            <el-menu-item
              v-for="submenu in menu.Childs"
              :index="submenu.Id + ''"
              :key="submenu.Id"
              @click="activeMenu(submenu)"
              >{{ submenu.Name }}</el-menu-item
            ></el-sub-menu>
        </el-menu>
      <el-container>
        <div>
          <el-header class="app-header" >
          <hamburger
            class="hamburger-container"
          />
          <AppNavbar />
        </el-header>
        <div  class="tabs-view-scroll">
            <el-tabs
              v-model="currentTabId"
              type="card"
              :closable="canClose"
              @tab-remove="removeMenuTab"
              @tab-click="changeTab"
            >
              <el-tab-pane
                :key="index"
                v-for="(item, index) in menuTabs"
                :label="item.title"
                :name="item.name"
                :closable="item.closable"
                :ref="item.ref"
              >
              </el-tab-pane>
            </el-tabs>
          </div>
        </div>
        <el-main style="padding: 5px">
          <router-view v-slot="{ Component }">
            <keep-alive  :key="router.currentRoute.value.path">
              <component :is="Component" />
            </keep-alive>
          </router-view>
        </el-main>
        <el-footer height="30px">Footer</el-footer>
      </el-container>
    </el-container>
  </el-container>
</template>

<script setup>
import { ref,onMounted, computed } from "vue";
import { useRouter, useRoute } from 'vue-router'
import {appState, appfunctions} from "@/store/glable.js"; //引入仓库'
import Hamburger from "@/layout/Hamburger/index.vue";
import AppNavbar from "@/layout/AppNavbar.vue";
import help from "@/utils/help.js";
import {Document, Menu as IconMenu,Location,Setting,} from '@element-plus/icons-vue'

const router = useRouter();

const currentTabId = ref("0")
const menuTabs=ref([])
const canClose = computed(() =>  menuTabs.value.length > 1)

const setHeight = ref(document.documentElement.clientHeight || document.body.clientHeight - 20)

const createMenu = ()=>{
  let path = router.currentRoute.value.path;
  let menuItem = appState.value.menus.find(x => x.Url.toLowerCase() == path.toLocaleLowerCase())
  if(!menuItem) {
    loadFirtMenu()
    return
  }
  let find = {
    id: menuItem.Id,
    title: menuItem.Name,
    name: menuItem.Id + "",
    closable: true,
    ref: "tabs_" + menuItem.Id,
    url: menuItem.Url,
  };
  menuTabs.value.push(find);
  currentTabId.value = find.name;
}

const loadFirtMenu = ()=>{
  let menuItem = appState.value.menutree[0]
  if(!menuItem) return
  if(menuItem.Url == ''){
    menuItem = menuItem.Childs[0]
  }
  let find = {
    id: menuItem.Id,
    title: menuItem.Name,
    name: menuItem.Id + "",
    closable: true,
    ref: "tabs_" + menuItem.Id,
    url: menuItem.Url,
  };
  menuTabs.value.push(find);
  activeMenu(menuItem)
}

const activeMenu =(menuItem)=> {
  let routes = router.getRoutes()
  let findIndex = routes.findIndex((x) => x.path == menuItem.Url);
  if (findIndex < 0) {
    help.warning("页面不存在");
    return;
  }
  router.push({ path: menuItem.Url }, () => {});
 
  var find = menuTabs.value.filter((tab) => tab.id === menuItem.Id);
  if (find.length > 0) {
    currentTabId.value = find[0].name;
  } else {
    try {
      find = {
        id: menuItem.Id,
        title: menuItem.Name,
        name: menuItem.Id + "",
        closable: (menuItem.Name.toLowerCase()==="home" || menuItem.Name =="首页")?false:true,
        ref: "tabs_" + menuItem.Id,
        url: menuItem.Url,
      };
      menuTabs.value.push(find);
      currentTabId.value = find.name;
    } catch (error) {
      help.error("页面不存在");
    }
  }
}

const removeMenuTab =(targetTabId) => {
  if(canClose.value == false) return false
  let nextTab = undefined;
  menuTabs.value.forEach((tab, index) => {
    if (tab.name === targetTabId) {

      nextTab = menuTabs.value[index + 1] || menuTabs.value[index - 1];
      if (nextTab) {
        currentTabId.value = nextTab.name;
        router.push({ path: nextTab.url }, () => {});
      }
    }
  });
  menuTabs.value = menuTabs.value.filter((tab) => tab.name !== targetTabId);
}

const changeTab =(targetTab) => {
  let currentTab = menuTabs.value.find((x) => x.name == targetTab.paneName);
  if (currentTab) {
    currentTabId.value = currentTab.name;
    router.push({ path: currentTab.url }, () => {});
  }
}

onMounted(() => {createMenu();  console.log(menuTabs.value)})

const sizeChange = () => {
  setHeight.value = document.documentElement.clientHeight || document.body.clientHeight - 20;
}
window.onresize = sizeChange;
</script>

<style lang="scss">

.app-logo{
  font-size: 20px;
  width: 100%;
  display: block;
  padding: 10px;
  color: #ffd04b;
  line-height: 45px;
}

.app-header{
  height: 30px;display: flex;justify-content: space-between;
}
.el-submenu .el-menu-item {
  min-width: 100px;
}
.el-menu-vertical {
  height: 100%;
}
.el-card__body,
.el-main {
  padding: 0px;
}
.el-tabs__header {
  margin: 0 0 5px;
}
$_svgWidth:2px;
$_tabsHeight: 30px;
.tabs-view-scroll {
    width: calc(100% - #{$_svgWidth});
    height: $_tabsHeight;
    top: 2px;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid #d8dce5;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, .12), 0 0 3px 0 rgba(0, 0, 0, .04);
    .el-tabs__nav-wrap.is-scrollable {
      padding: 0 $_svgWidth + 1px;
    }
    .el-tabs__header {
      margin: 0;
      border-bottom: 0;
    }
    .el-tabs__content {
      display: none;
    }
    .el-tabs__nav {
      border: none !important;
    }
    .el-tabs__nav-next,
    .el-tabs__nav-prev {
      line-height: $_tabsHeight - 4px;
      font-size: 14px;
      width: $_svgWidth;
      text-align: center;
      &:not(.is-disabled):hover {
        color: #000;
      }
      &.is-disabled {
        opacity: 0.4;
        cursor: not-allowed;
      }
    }
    .el-tabs__item {
      height: $_tabsHeight - 4px ;
      line-height: $_tabsHeight - 5px;
      font-size: 12px;
      margin: 0 2px;
      border: 1px solid #d9d9d9 !important;
      border-radius: 2px;
      padding: 0 10px !important;
      &:first-child {
        border-left: 1px solid #d9d9d9;
      }
      &:last-child {
        border-right: 1px solid #d9d9d9;
      }
    }
  }
</style>
