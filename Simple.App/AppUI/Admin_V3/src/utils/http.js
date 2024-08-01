import help from '@/utils/help'
import  axios  from  'axios'
import  appconfig from '@/appconfig'
import {appState} from "@/store/glable.js"; //引入仓库'

let loadingInstance = {}

const http = axios.create({
    baseURL: appconfig.baseURL,
    headers:{},//请求头
    settimeout:5000,//超时时间
    withCredentials :false
});

function getToken(){
    return appState.value.token;
}

// 添加请求拦截器
http.interceptors.request.use(
    config => {
        loadingInstance = help.loading();
        // 在发送请求之前做些什么,此处设置Authorization的参数值 ，用做鉴权参数
        config.headers["Authorization"] = getToken() ;
        return config;
    },
    error => {
        // 对请求错误做些什么
        console.error(error);
        help.error('请求数据异常！')
        loadingInstance.close();
        return Promise.reject("请求出错了", error);
    }
);

// 添加响应拦截器
http.interceptors.response.use(
    response => {
        loadingInstance.close();
        // 对响应数据做点什么
        // console.log("返回的数据", response);
        switch (response.data.code) {
            case 200:
                return response.data;
            default:
                help.warning(response.data.message || '请求数据异常！');
                return Promise.reject(new Error(response.data.message || '请求数据异常！'))
        }
    },
    error => {
        loadingInstance.close();
        if(error.request){
            switch (error.request.status) {
                case 404:
                    help.error('请求地址不存在！')
                    break;
                case 403:
                    help.error('系统拒绝访问')
                  break;
                case 401:
                    help.error('请重新登录')
                    window.location.pathname = '/login'
                    break;
                default:
                    help.error('服务器异常 '+ error.message)
                    return Promise.reject(error);
                }
        }else{
            help.error('其他异常：' + error.message)
        }
        // 对响应错误做点什么
        return Promise.reject(error);
    }
);
export default http;
