<template>
  <div class="loginbody">
    <div class="logindata">
      <div class="logintext">
        <h2>Welcome</h2>
      </div>
      <div class="formdata">
        <el-form ref="editform" :model="form" :rules="rules">
          <el-form-item prop="account">
            <el-input
              v-model="form.account"
              clearable
              placeholder="请输入账号"
            ></el-input>
          </el-form-item>
          <el-form-item prop="password">
            <el-input
              v-model="form.password"
              clearable
              placeholder="请输入密码"
              show-password
            ></el-input>
          </el-form-item>
        </el-form>
      </div>
      <div class="tool">
        <div>
          <el-checkbox v-model="recordPwd" @change="remenber"
            >记住密码</el-checkbox
          >
        </div>
        <div>
          <span class="shou" @click="forgetpas">忘记密码？</span>
        </div>
      </div>
      <div class="butt">
        <el-button type="primary" @click.prevent="loginAction(editform)"
          >登录</el-button
        >
        <el-button class="shou" @click="register">注册</el-button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { login, usermenus } from "@/api/sys";
import { ref } from "vue";
import { useRouter, useRoute } from 'vue-router'
import help from "@/utils/help";

import {appState,appfunctions} from "@/store/glable.js"; //引入仓库'
import { de } from "element-plus/es/locale/index.mjs";

const router = useRouter()
const form = ref({
        account: "admin",
        password: "123456",
      })
const recordPwd = ref(false)
const rules = {
        account: [
          { required: true, message: "请输入用户名", trigger: "blur" },
          { max: 10, message: "不能大于10个字符", trigger: "blur" },
        ],
        password: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { max: 10, message: "不能大于10个字符", trigger: "blur" },
        ],
      }
const editform = ref(null)

const loginAction =(formtmp)=> {
    formtmp.validate((valid,fields) => {
      if (valid) {
        login(form.value).then((res) => {
          appState.value.user = res.data
          appState.value.token = res.data.Token;
          appState.value.isLoggedIn = true;
          usermenus().then((resmenu) => {
            appState.value.menus = resmenu.data.menus;
            resmenu.data.menuTree.forEach((item) => item.isActived = false)
            appState.value.menutree = resmenu.data.menuTree;
            appState.value.functions = resmenu.data.functions;
            router.push('/')
          });
        });
      } else {
        return false;
      }
    });
  }
  const  remenber = (data)=> {
      recordPwd.value  = data;
      if (recordPwd.value) {
        localStorage.setItem("news", JSON.stringify(form.value));
      } else {
        localStorage.removeItem("news");
      }
    }
  const  forgetpas= ()=> {
    help.info("功能尚未开发额😥");
    }
  const  register =()=> { }
 

</script>

<style scoped>
.loginbody {
  width: 100%;
  height: 100%;
  min-width: 1000px;
  background-size: 100% 100%;
  background-position: center center;
  overflow: auto;
  background-repeat: no-repeat;
  position: fixed;
  line-height: 100%;
  padding-top: 150px;
  background: linear-gradient(to right, #0984d9, #a6c1ee);
}

.logintext {
  margin-bottom: 20px;
  line-height: 50px;
  text-align: center;
  font-size: 30px;
  font-weight: bolder;
  color: white;
  text-shadow: 2px 2px 4px #000000;
}

.logindata {
  width: 400px;
  height: 300px;
  transform: translate(-50%);
  margin-left: 61%;
}

.tool {
  display: flex;
  justify-content: space-between;
  color: #606266;
}

.butt {
  margin-top: 10px;
  text-align: center;
}

.shou {
  cursor: pointer;
  color: #606266;
}

/*ui*/
/* /deep/ .el-form-item__label {
  font-weight: bolder;
  font-size: 15px;
  text-align: left;
}

/deep/ .el-button {
  width: 100%;
  margin-bottom: 10px;

} */
</style>
