import { Message } from 'element-ui'
const tokenKey = 'token'
//help帮助类 比如缓存 或者一些常用的静态方法
const help = {
  //数据缓存方法
  data: {
    set: function (key, value) {
      localStorage.setItem(key, JSON.stringify(value))
    },
    //获取数据 remove 是否获取后删除
    get: function (key, remove = false) {
      let value = localStorage.getItem(key)
      if (remove) localStorage.removeItem(key)
      return JSON.parse(value)
    },
  },
  gettoken: function () {
    let token = help.data.get(tokenKey)
    return token
  },
  settoken: function (value) {
    return this.data.set(tokenKey, value)
  },
  editFormat: function (param) {
    const data = Object.assign({}, param)
    delete data['Id']
    delete data['IsDeleted']
    return data
  },

  success: function (message) {
    Message.success(message)
  },
  info: function (message) {
    Message.info(message)
  },
  error: function (message) {
    Message.error(message)
  },
  warning: function (message) {
    Message.warning(message)
  },
  showRes: function (res) {
    if (res.code === 200) {
      Message.success(res.message)
    } else {
      Message.error(res.message)
    }
  },
}
export default help
