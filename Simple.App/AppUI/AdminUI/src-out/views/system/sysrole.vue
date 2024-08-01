<template>
  <el-container>
    <el-main>
      <el-row style="padding: 10px" type="flex">
        <el-button-group>
          <el-button type="primary" icon="el-icon-edit" @click="add" size="mini"
            >新增</el-button
          >
        </el-button-group>
      </el-row>
      <el-table
        :data="datas"
        border
        size="small"
        :highlight-current-row="true"
        style="width: 100%; margin-top: 10px"
      >
        <el-table-column prop="Name" label="名称" width="160"></el-table-column>
        <el-table-column prop="Remark" label="备注"></el-table-column>
        <el-table-column fixed="right" label="操作" align="center" width="150">
          <template v-slot="scope">
            <el-button
              @click="SetFlag(scope.row)"
              type="text"
              style="color: #e6a23c"
              size="small"
              >{{ scope.row.Enabled ? '启用' : '停用' }}</el-button
            >
            <el-button type="text" size="small" @click="editauth(scope.row)"
              >授权</el-button
            >
            <el-button type="text" size="small" @click="edit(scope.row)"
              >编辑</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <!-- 授权页面 -->
    <el-dialog
      title="权限编辑"
      v-model:visible="showAuthorityForm"
      :closeOnClickModal="false"
      width="400px"
      :append-to-body="true"
      destroy-on-close
      @opened="authorityFormOpened"
    >
      <el-tree
        :data="menus"
        show-checkbox
        node-key="Id"
        default-expand-all
        ref="menu_tree"
        :props="{ children: 'Childs', label: 'Name' }"
        :expand-on-click-node="false"
      >
      </el-tree>
      <template v-slot:footer>
        <div class="dialog-footer">
          <el-button @click="showAuthorityForm = false">取 消</el-button>
          <el-button type="primary" @click="saveAuthData">确 定</el-button>
        </div>
      </template>
    </el-dialog>
    <!-- 编辑窗口 -->
    <el-dialog
      title="角色编辑"
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
        class="demo-ruleForm"
      >
        <el-form-item prop="Name" label="名称" required>
          <el-input type="text" v-model:value="eidtModel.Name"></el-input>
        </el-form-item>
        <el-form-item prop="Remark" label="备注">
          <el-input
            style="width: 100%"
            type="textarea"
            v-model:value="eidtModel.Remark"
          ></el-input>
        </el-form-item>
      </el-form>
      <template v-slot:footer>
        <div class="dialog-footer">
          <el-button @click="cancelEdit">取 消</el-button>
          <el-button type="primary" @click="saveData">确 定</el-button>
        </div>
      </template>
    </el-dialog>
  </el-container>
</template>

<script>
import { qRole, dalRole, qMenu } from '@/api/sys'
import { mapGetters } from 'vuex'
export default {
  name: 'sysrole',
  computed: {
    ...mapGetters(['info']),
  },
  data() {
    return {
      datas: [],
      menus: [], //权限数据
      eidtModel: { Code: '', Name: '', Remark: '' },
      currentData: {}, //当前操作对象
      showEditForm: false, //是否显示编辑窗
      showAuthorityForm: false, //是否显示授权窗口
      formLabelWidth: '120px',
      rules: {
        Name: [{ required: true, message: '名称必填', trigger: 'change' }],
        Code: [{ required: true, message: '值Code必填', trigger: 'change' }],
      },
    }
  },
  mounted() {
    this.getDatas()
    this.getMenu()
  },
  methods: {
    add() {
      //新增
      this.eidtModel = { Code: '', Name: '', Remark: '' }
      this.showEditForm = true
    },
    authorityFormOpened() {
      //授权页面打开后的事件
      let me = this
      let checkAuthKeys = []
      me.currentData.SysRoleAuthorities.forEach((authority) => {
        checkAuthKeys.push(authority.AuthorityId)
      })
      if (checkAuthKeys.length > 0) {
        this.$refs.authority_tree.setCheckedKeys(checkAuthKeys)
      }
    },
    editauth(model) {
      //授权
      let me = this
      me.currentData = model
      me.showAuthorityForm = true
    },
    saveAuthData() {
      //保存授权
      let me = this
      let checkedKey = me.$refs.authority_tree.getCheckedKeys()
      saveRoleAuth(me.currentData.Id, checkedKey).then((resp) => {
        if (resp.success) {
          me.$message('保存成功！')
          me.getDatas()
          me.showAuthorityForm = false
        }
      })
    },
    edit(model) {
      //编辑
      let me = this
      this.currentData = model
      for (const key in me.eidtModel) {
        if (model.hasOwnProperty(key)) {
          me.eidtModel[key] = this.currentData[key]
        }
      }
      this.showEditForm = true
    },
    cancelEdit() {
      this.showEditForm = false
      this.getDatas()
    },
    saveData() {
      //保存数据
      let me = this

      this.showEditForm = true
      this.$refs['editDataForm'].validate((valid) => {
        if (valid) {
          if (me.currentData.Id > 0) {
            //新增
            dalRole('edit/' + me.currentData.Id, me.eidtModel, 'put').then(
              (resp) => {
                me.$help.showRes(resp)
                if (resp.success) {
                  me.getDatas()
                  me.cancelEdit()
                }
              }
            )
          } else {
            //新增
            dalRole('add', me.eidtModel).then((resp) => {
              me.$help.showRes(resp)
              if (resp.success) {
                me.getDatas()
                me.cancelEdit()
              }
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
        `确定要${model.IsDeleted ? '启用' : '停用'}此记录吗？`,
        '询问',
        {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning',
        }
      )
        .then(() => {
          dalRole('setflag/' + model.Id, { Enabled: !model.Enabled }, 'put')
            .then((resp) => {
              if (resp.success) {
                model.Enabled = !model.Enabled
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
      qRole('list').then((resp) => {
        me.datas = resp.data
      })
    },
    getMenu() {
      let me = this
      qMenu('menuTree').then((resp) => {
        me.menus = resp.data
      })
    },
  },
}
</script>
