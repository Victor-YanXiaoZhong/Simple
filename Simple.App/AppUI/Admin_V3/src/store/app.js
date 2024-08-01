import { defineStore } from "pinia"
 
const useAppInfoStore = defineStore('appInfo', {
  state: () => ({
    sidebar: {
        opened: true,
        withoutAnimation: false
    },
    device: 'desktop',
    menus:[],
    menuTree:[],
    auths:[]
  }),
})
export default useAppInfoStore