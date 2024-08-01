<template>
    <el-container>
      <!-- <el-aside></el-aside> -->
      <el-main>
        <el-card class="box-card">
          <template v-slot:header>
            <div class="clearfix">
              <span>个人信息：</span>
              <el-button
                style="float: right; padding: 3px 0"
                type="text"
                @click="edit"
                >编辑</el-button
              >
            </div>
          </template>
          <div class="text item">
            账户：<span class="info">{{ appState.user.Account }}</span>
          </div>
          <div class="text item">
            名称：<span class="info">{{ appState.user.Name }}</span>
          </div>
          <div class="text item">
            电话：<span class="info">{{ appState.user.Phone }}</span>
          </div>
          <div class="text item">
            邮箱：<span class="info">{{ appState.user.Email }}</span>
          </div>
          <div class="text item">
            机构：<span class="info">{{ appState.user.OrgnizationName }}</span>
          </div>
          <div class="text item">
            角色：<span class="info">{{ appState.user.RolesName }}</span>
          </div>
        </el-card>
      </el-main>
      <!-- 编辑窗口 -->
      <el-dialog
        title="信息编辑"
        v-model="showEditForm"
        :closeOnClickModal="false"
        width="700px"
        :append-to-body="true"
        destroy-on-close
      >
        <el-form
          :model="eidtModel"
          :rules="rules"
          ref="editDataForm"
          label-width="120px"
          size="small"
          :inline="true"
          class="demo-ruleForm"
        >
          <el-form-item prop="Name" label="名称" required>
            <el-input type="text" v-model="eidtModel.Name"></el-input>
          </el-form-item>
          <el-form-item prop="Email" label="邮箱" required>
            <el-input type="text" v-model="eidtModel.Email"></el-input>
          </el-form-item>
          <el-form-item prop="Contract" label="联系人">
            <el-input type="text" v-model="eidtModel.Contract"></el-input>
          </el-form-item>
          <el-form-item prop="Phone" label="电话">
            <el-input type="text" v-model="eidtModel.Phone"></el-input>
          </el-form-item>
        </el-form>
        <template v-slot:footer>
          <div class="dialog-footer">
            <el-button @click="showEditForm = false">取 消</el-button>
            <el-button type="primary" @click="saveData">确 定</el-button>
          </div>
        </template>
      </el-dialog>
    </el-container>
  </template>
  
  <script setup>
  import { dalUser,userinfo } from '@/api/sys'
  import {appState, appfunctions} from "@/store/glable.js"; //引入仓库'
  import { ref } from 'vue';

  const eidtModel = ref({ Name: '', Email: '', Contract: '', Phone: '' }) //编辑模型
  const showEditForm = ref(false) //是否显示编辑窗
  const editDataForm = ref(null) //编辑表单
  const formLabelWidth = ref('120px') //表单项宽度
  const rules = {
    Name: [{ required: true, message: '名称必填', trigger: 'change' }],
    Email: [{ required: true, message: '邮箱必填', trigger: 'change' }],
  }

  const edit = () => {
    //编辑
    for (const key in eidtModel.value) {
      if (appState.value.user.hasOwnProperty(key)) {
        eidtModel.value[key] = appState.value.user[key]
      }
    }
    showEditForm.value = true
  }
  const saveData = ()=>{
    //保存数据
    showEditForm.value = false
    editDataForm.value.validate((valid) => {
      if (valid) {
        dalUser('edit/' + appState.value.user.Id, eidtModel.value, 'put').then((resp) => {
          if (resp.success) {
            appfunctions.help.success('保存成功！')
            appState.value.user.Name = eidtModel.value.Name
            appState.value.user.Email = eidtModel.value.Email
            appState.value.user.Contract = eidtModel.value.Contract
            appState.value.user.Phone = eidtModel.value.Phone
          } else {
            appfunctions.help.error('保存失败！')
          }
        })
      }
    })
  }
  </script>
  
  <style>
  .text {
    font-size: 14px;
  }
  .item {
    padding: 18px 10px;
  }
  .info {
    color: #409eff;
  }
  </style>
  