<template>
  <el-container :style="{ height: setHeight + 'px' }">
    <el-container>
      <el-aside width="200px" v-show="sidebar.opened">
        <el-menu
          default-active="2"
          class="el-menu-vertical"
          background-color="#545c64"
          text-color="#fff"
          active-text-color="#ffd04b"
        >
          <el-submenu
            :index="menu.Id + ''"
            v-for="menu in menus"
            :key="menu.Id + ''"
          >
            <template v-slot:title>
              <i class="el-icon-location"></i>
              <span>{{ menu.Name }}</span>
            </template>
            <el-menu-item
              v-for="submenu in menu.Childs"
              :index="submenu.Id + ''"
              :key="submenu.Id"
              @click="activeMenu(submenu)"
              >{{ submenu.Name }}</el-menu-item
            >
          </el-submenu>
        </el-menu>
      </el-aside>
      <el-container>
        <el-header height="30px">
          <hamburger
            :is-active="sidebar.opened"
            class="hamburger-container"
            @toggleClick="toggleSideBar"
          />
        </el-header>
        <el-main style="padding: 5px">
          <el-tabs
            v-model:value="currentTabName"
            type="card"
            closable
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
          <keep-alive max="50">
            <router-view :key="$route.fullPath" />
          </keep-alive>
        </el-main>
        <el-footer height="30px">Footer</el-footer>
      </el-container>
    </el-container>
  </el-container>
</template>

<script>
import { mapGetters } from 'vuex'
import Hamburger from './Hamburger'
export default {
  name: 'AppLayout',
  components: { Hamburger },
  data() {
    return {
      currentTabName: '0',
      menuTabs: [],
    }
  },
  computed: {
    ...mapGetters(['menus', 'sidebar']),
    setHeight() {
      return (
        (document.documentElement.clientHeight || document.body.clientHeight) -
        20
      )
    },
  },
  created() {
    let me = this
    if (me.menuTabs.length == 0) {
      let path = me.$route.path
      let menuItem = undefined
      for (let i = 0; i < me.menus.length; i++) {
        if (me.menus[i].Url == path) {
          menuItem = me.menus[i]
          break
        }
        for (let j = 0; j < me.menus[i].Childs.length; j++) {
          if (me.menus[i].Childs[j].Url == path) {
            menuItem = me.menus[i].Childs[j]
            break
          }
        }
      }
      if (!menuItem) return

      let find = {
        id: menuItem.Id,
        title: menuItem.Name,
        name: menuItem.Id + '',
        closable: true,
        ref: 'tabs_' + menuItem.Id,
        url: menuItem.Url,
      }
      me.menuTabs.push(find)
      me.currentTabName = find.name
    }
  },
  methods: {
    toggleSideBar() {
      this.$store.dispatch('app/toggleSideBar')
    },
    activeMenu(menuItem) {
      let me = this
      let findIndex = me.$router.matcher
        .getRoutes()
        .findIndex((x) => x.path == menuItem.Url)
      if (findIndex < 0) {
        me.$help.warning('页面不存在')
        return
      }
      me.$router.push({ path: menuItem.Url }, () => {})
      let tabs = this.menuTabs
      var find = tabs.filter((tab) => tab.id === menuItem.Id)
      if (find.length > 0) {
        this.currentTabName = find[0].name
      } else {
        try {
          find = {
            id: menuItem.Id,
            title: menuItem.Name,
            name: menuItem.Id + '',
            closable: true,
            ref: 'tabs_' + menuItem.Id,
            url: menuItem.Url,
          }
          tabs.push(find)
          this.currentTabName = find.name
        } catch (error) {
          me.$help.error('页面不存在')
        }
      }
    },
    removeMenuTab(targetName) {
      let tabs = this.menuTabs
      let activeName = this.currentTabName

      tabs.forEach((tab, index) => {
        if (tab.name === targetName) {
          let nextTab = tabs[index + 1] || tabs[index - 1]
          if (nextTab) {
            activeName = nextTab.name
          }
        }
      })

      this.currentTabName = activeName
      this.menuTabs = tabs.filter((tab) => tab.name !== targetName)
    },
    changeTab(targetName) {
      let me = this
      let currentTab = me.menuTabs.find((x) => x.name == targetName.paneName)
      if (currentTab) {
        me.currentTabName = currentTab.name
        me.$router.push({ path: currentTab.url }, () => {})
      }
    },
  },
}
</script>

<style>
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
</style>
