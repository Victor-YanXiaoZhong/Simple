<template>
    <div>
      <el-row type="flex"
        ><el-button
          type="success"
          style="margin: 5px 0"
          @click="add()"
          >添加根节点</el-button
        ></el-row
      >
      <el-table
        :data="menus"
        style="width: 100%; margin-bottom: 20px"
        row-key="Id"
        border
        :tree-props="{ children: 'Childs' }"
      >
        <el-table-column prop="Name" label="名称" width="250"></el-table-column>
        <el-table-column prop="Url" label="链接"></el-table-column>
        <el-table-column prop="IsShow" label="是否显示" width="100">
          <template v-slot="scope">{{
            scope.row.IsShow ? '显示' : '隐藏'
          }}</template>
        </el-table-column>
        <el-table-column prop="IsClose" label="能否关闭" width="100">
          <template v-slot="scope">{{
            scope.row.IsClose ? '不能' : '能'
          }}</template>
        </el-table-column>
        <el-table-column prop="Icon" label="图标" width="80"></el-table-column>
        <el-table-column prop="Sort" label="排序号" width="100"></el-table-column>
        <el-table-column fixed="right" label="操作" align="center" width="220">
          <template v-slot="scope">
            <el-button @click="SetFlag(scope.row)" type="warning" link
              >{{ scope.row.Deleted ? '启用' : '停用' }}</el-button
            >
            <el-button type="primary"  @click="edit(scope.row)" link
              >编辑</el-button
            >
            <el-button type="success" @click="addChildren(scope.row)" link
              >新增子项</el-button
            >
          </template>
        </el-table-column>
      </el-table>
      <!-- 编辑窗口 -->
      <el-dialog
        title="编辑"
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
        >
          <el-form-item label="上级菜单" required>
            <el-tag type="success">{{ parent.Name }}</el-tag>
          </el-form-item>
          <el-form-item prop="Name" label="名称" required>
            <el-input type="text" v-model="eidtModel.Name"></el-input>
          </el-form-item>
          <el-form-item prop="Url" label="链接">
            <el-input type="text" v-model="eidtModel.Url" placeholder="主菜单不需要输入url"></el-input>
          </el-form-item>
          <el-form-item prop="Icon" label="图标">
            <el-input type="text" v-model="eidtModel.Icon"></el-input>
          </el-form-item>
          <el-form-item prop="Sort" label="排序号">
            <el-input type="number" v-model="eidtModel.Sort"></el-input>
          </el-form-item>
          <el-form-item prop="IsShow" label="显示菜单">
            <el-switch
              v-model="eidtModel.IsShow"
              active-color="#13ce66"
              inactive-color="lightgray"
            ></el-switch>
          </el-form-item>
        </el-form>
        <template v-slot:footer>
          <div class="dialog-footer">
            <el-button @click="showEditForm = false">取 消</el-button>
            <el-button type="primary" @click="saveData">确 定</el-button>
          </div>
        </template>
      </el-dialog>
    </div>
  </template>
  
<script setup>
  import { ref,onMounted } from 'vue'
  import { qMenu, dalMenu } from '@/api/sys'
  import help from '@/utils/help'

  const menus = ref([])
  const currentData = ref({ Id: 0 })
  const parent = ref({ Id: 0, Name: '根菜单' })
  const eidtModel = ref({
    ParentId: 0,
    Name: '',
    Url: '',
    Icon: '',
    Sort: 0,
    IsShow: true,
    IsClose: false,
  })
  const showEditForm = ref(false)
  const formLabelWidth = ref('120px')
  const editDataForm = ref(null)

  const checkUrl = (rule, value, callback) => {
    if (value.trim() == '' && eidtModel.value.ParentId != 0) {
      callback(new Error('子菜单路由Url必填'))
    } else {
      callback()
    }
  }

  const rules = ref({
    Sort: [{ required: true, message: 'Sort必填', trigger: 'change' }],
    Name: [{ required: true, message: '名称必填', trigger: 'change' }],
    ParentId: [{ required: true, message: 'ParentId必填', trigger: 'change' }],
    Url: [{ validator:checkUrl, trigger: 'change' }],
  })
  onMounted(() => {
    getDatas()
  })


  const getDatas =()=> {
    qMenu('menuTree').then((resp) => {
      menus.value = resp.data
    })
  } 

  const addChildren = (model) => {
    //添加子项
    eidtModel.value = {
      ParentId: 0,
      Name: '',
      Url: '',
      Icon: '',
      Sort: 0,
      IsShow: true,
      IsClose: false,
    }
    currentData.value['Id'] = 0
    eidtModel.value.ParentId = model.Id
    parent.value = { Id: model.Id, Name: model.Name }
    showEditForm.value = true
  }

  const add = () => {
    //新增
    showEditForm.value = true
    eidtModel.value = {
      ParentId: 0,
      Name: '',
      Url: '',
      Icon: '',
      Sort: 0,
      IsShow: true,
      IsClose: false,
    }
  }

  const edit = (model) => {
    //编辑
    currentData.value = model
    if (model.ParentId == 0) {
      parent.value = { Id: 0, Name: '根菜单' }
    } else {
      let parent = menus.value.find((x) => (x.Id = model.ParentId))
      parent.value = { Id: parent.Id, Name: parent.Name }
    }
    for (const key in eidtModel.value) {
      if (model.hasOwnProperty(key)) {
        eidtModel.value[key] = model[key]
      }
    }
    showEditForm.value = true
  }

  const cancelEdit = () => {
    showEditForm.value = false
    eidtModel.value = {
      ParentId: 0,
      Name: '根节点',
      Url: '',
      Icon: '',
      Sort: 0,
      IsShow: true,
      IsClose: false,
    }
    getDatas()
  }

  const saveData = () => {
    //保存数据
    showEditForm.value = true
    editDataForm.value.validate((valid) => {
      if (valid) {
        if (currentData.value.Id > 0) {
          //保存
          dalMenu('edit/' + currentData.value.Id, eidtModel.value, 'put').then(
            (resp) => {
              help.showRes(resp)
              cancelEdit()
            }
          )
        } else {
          //新增
          dalMenu('add', eidtModel.value).then((resp) => {
            help.showRes(resp)
            cancelEdit()
          })
        }
      } else {
        help.warning('请完善必填项后再继续')
      }
    })
  }

  const SetFlag = (model) => {
    //设置停用启用的标量值
    help.confirm(
      `确定要${model.Deleted ? '启用' : '停用'}此记录吗？`,()=>{
        dalMenu('setflag/' + model.Id, { Deleted: !model.Deleted }, 'put')
          .then((resp) => {
            if (resp.success) {
              model.Deleted = !model.Deleted
              help.showRes(resp)
            }
          })
          .catch((err) => {
            help.warning('设置没有成功!')
          })
      }
    )
  }
  </script>
  