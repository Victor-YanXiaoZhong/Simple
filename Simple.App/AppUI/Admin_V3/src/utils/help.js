import { ElLoading,ElMessage,ElMessageBox } from 'element-plus';

const tokenKey = 'token';
let loadingInstance = undefined;
let timerId = undefined;
//help帮助类 比如缓存 或者一些常用的静态方法
const help = {
    //数据缓存方法
    data:{
        set : function(key,value){
            localStorage.setItem(key,JSON.stringify(value));
        },
        //获取数据 remove 是否获取后删除
        get : function(key,remove = false){
            let value = localStorage.getItem(key);
            if(remove) localStorage.removeItem(key);
            return JSON.parse(value);
        }
    },
    gettoken : function(){
      let token = help.data.get(tokenKey)
      return token
    },
    settoken : function(value){
      return this.data.set(tokenKey,value)
    },
    editFormat :function(param) {
      const data = Object.assign({}, param)
      delete data['Id']
      delete data['IsDeleted']
      return data
    },

    success :function(message) {
      ElMessage.success(message)
    },
    info :function(message) {
      ElMessage.info(message)
    },
    error : function(message) {
      ElMessage.error(message)
    },
    warning : function(message) {
      ElMessage.warning(message)
    },
    showRes :function (res) {
      if(res.code === 200){
        ElMessage.success(res.message)
      }else{
        ElMessage.error(res.message)
      }
    },
    confirm : function(message,callback){
      ElMessageBox.confirm(message,'提示',{
        type: 'warning',
        callback: (action) => {
          if(action === 'confirm'){
            callback()
          }
        }
      })
    },
    loading:(message="请求中",timeClose=0)=>{
      loadingInstance = ElLoading.service({
        lock: true,
        text: message,
        background: 'rgba(0, 0, 0, 0.7)',
      });
      if(timeClose > 0){
        timerId = setTimeout(() => {
          help.closeLoading()
          clearTimeout(timerId)
        }, timeClose * 1000);
      }
      return loadingInstance;
    },
}
export default help;
