import {ref} from 'vue'
import help from '../utils/help'
//公用对象
export const  appState = ref({
    user: undefined,
    token: "",
    isLoggedIn: false,
    isOpenSidebar: true,
    menus:[],
    menutree:[],
    functions:[]

})
//公用方法
export const appfunctions = {
    setUser: (user) => {
        appState.value.user = user
        appState.value.isLoggedIn = true},
    setToken: (token) => {
        appState.value.token = token},
    setMenus: (menus) => {
        appState.value.menus = menus},
    toggleSidebar: () => {
        appState.value.isOpenSidebar =!appState.value.isOpenSidebar},
    saveState: () => {
        sessionStorage.setItem('appState', JSON.stringify(appState.value))
    },
    loadState: () => {appState.value = JSON.parse(sessionStorage.getItem('appState')) || appState.value},
    logout: () => {
        appState.value.user = undefined
        appState.value.token = ""
        appState.value.isLoggedIn = false
        appState.value.isOpenSidebar = true
        appState.value.menus = []
        appState.value.menutree = []
        appState.value.functions = []
        sessionStorage.removeItem('appState')
    },help
}