<template>
  <div>
    <el-row type="flex"
      ><el-button
        type="primary"
        style="margin: 5px 0"
        @click="add()"
        size="mini"
        >添加根节点</el-button
      ></el-row
    >
    <el-table
      :data="menus"
      style="width: 100%; margin-bottom: 20px"
      row-key="Id"
      border
      size="mini"
      :tree-props="{ children: 'Childs' }"
    >
      <el-table-column prop="Name" label="名称" width="200"></el-table-column>
      <el-table-column prop="Url" label="链接" width="200"></el-table-column>
      <el-table-column prop="IsShow" label="是否显示" width="80">
        <template v-slot="scope">{{
          scope.row.IsShow ? '显示' : '隐藏'
        }}</template>
      </el-table-column>
      <el-table-column prop="IsClose" label="能否关闭" width="80">
        <template v-slot="scope">{{
          scope.row.IsClose ? '不能' : '能'
        }}</template>
      </el-table-column>
      <el-table-column prop="Icon" label="图标" width="80"></el-table-column>
      <el-table-column prop="Sort" label="排序号" width="60"></el-table-column>
      <el-table-column fixed="right" label="操作" align="center" width="220">
        <template v-slot="scope">
          <el-button
            @click="SetFlag(scope.row)"
            type="text"
            style="color: #e6a23c"
            size="small"
            >{{ scope.row.Deleted ? '启用' : '停用' }}</el-button
          >
          <el-button type="text" size="small" @click="edit(scope.row)"
            >编辑</el-button
          >
          <el-button type="text" size="small" @click="addChildren(scope.row)"
            >新增子项</el-button
          >
        </template>
      </el-table-column>
    </el-table>
    <!-- 编辑窗口 -->
    <el-dialog
      title="编辑"
      v-model:visible="showEditForm"
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
      >
        <el-form-item label="上级菜单" required>
          <el-tag type="success">{{ parent.Name }}</el-tag>
        </el-form-item>
        <el-form-item prop="Name" label="名称" required>
          <el-input type="text" v-model:value="eidtModel.Name"></el-input>
        </el-form-item>
        <el-form-item prop="Url" label="链接" required>
          <el-input type="text" v-model:value="eidtModel.Url"></el-input>
        </el-form-item>
        <el-form-item prop="Icon" label="图标">
          <el-input type="text" v-model:value="eidtModel.Icon"></el-input>
        </el-form-item>
        <el-form-item prop="Sort" label="排序号">
          <el-input type="number" v-model:value="eidtModel.Sort"></el-input>
        </el-form-item>
        <el-form-item prop="IsShow" label="显示菜单">
          <el-switch
            v-model:value="eidtModel.IsShow"
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

<script>
import { qMenu, dalMenu } from '@/api/sys'
export default {
  name: 'SysMenu',
  data() {
    return {
      menus: [], //所有权限树结构
      currentData: { Id: 0 },
      parent: { Id: 0, Name: '根菜单' },
      eidtModel: {
        ParentId: 0,
        Name: '',
        Url: '',
        Icon: '',
        Sort: 0,
        IsShow: true,
        IsClose: false,
      },
      showEditForm: false, //是否显示编辑窗
      formLabelWidth: '120px',
      rules: {
        Sort: [{ required: true, message: 'Sort必填', trigger: 'change' }],
        Name: [{ required: true, message: '名称必填', trigger: 'change' }],
        ParentId: [
          { required: true, message: 'ParentId必填', trigger: 'change' },
        ],
        Url: [{ required: true, message: '路由Url必填', trigger: 'change' }],
      },
    }
  },
  mounted() {
    this.getDatas()
  },
  methods: {
    addChildren(model) {
      //添加子项
      let me = this
      this.eidtModel = {
        ParentId: 0,
        Name: '',
        Url: '',
        Icon: '',
        Sort: 0,
        IsShow: true,
        IsClose: false,
      }
      me.currentData['Id'] = 0
      me.eidtModel.ParentId = model.Id
      me.parent = { Id: model.Id, Name: model.Name }
      this.showEditForm = true
    },
    add() {
      //新增
      this.showEditForm = true
      this.eidtModel = {
        ParentId: 0,
        Name: '',
        Url: '',
        Icon: '',
        Sort: 0,
        IsShow: true,
        IsClose: false,
      }
    },
    edit(model) {
      //编辑
      let me = this
      me.currentData = model
      if (model.ParentId == 0) {
        me.parent = { Id: 0, Name: '根菜单' }
      } else {
        let parent = me.menus.find((x) => (x.Id = model.ParentId))
        me.parent = { Id: parent.Id, Name: parent.Name }
      }
      for (const key in me.eidtModel) {
        if (model.hasOwnProperty(key)) {
          me.eidtModel[key] = model[key]
        }
      }
      this.showEditForm = true
    },
    cancelEdit() {
      this.showEditForm = false
      this.eidtModel = {
        ParentId: 0,
        Name: '',
        Url: '',
        Icon: '',
        Sort: 0,
        IsShow: true,
        IsClose: false,
      }
      this.getDatas()
    },
    saveData() {
      //保存数据
      let me = this

      this.showEditForm = true
      this.$refs['editDataForm'].validate((valid) => {
        if (valid) {
          if (me.currentData.Id > 0) {
            //保存
            dalMenu('edit/' + me.currentData.Id, me.eidtModel, 'put').then(
              (resp) => {
                me.$help.showRes(resp)
                me.cancelEdit()
              }
            )
          } else {
            //新增
            dalMenu('add', me.eidtModel).then((resp) => {
              me.$help.showRes(resp)
              me.cancelEdit()
            })
          }
        } else {
          me.$message({
            message: '请完善必填项后再继续',
            type: 'warning',
          })
        }
      })
    },
    SetFlag(model) {
      //设置停用启用的标量值
      let me = this
      this.$confirm(
        `确定要${model.Deleted ? '启用' : '停用'}此记录吗？`,
        '询问',
        {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning',
        }
      )
        .then(() => {
          dalMenu('setflag/' + model.Id, { Deleted: !me.Deleted }, 'put')
            .then((resp) => {
              if (resp.success) {
                model.Deleted = !model.Deleted
                me.$help.showRes(resp)
              }
            })
            .catch((err) => {
              me.$message({
                type: 'waring',
                message: '设置没有成功!',
              })
            })
        })
        .catch(() => {})
    },
    getDatas() {
      let me = this
      qMenu('menuTree').then((resp) => {
        me.menus = resp.data
      })
    },
  },
}
</script>
