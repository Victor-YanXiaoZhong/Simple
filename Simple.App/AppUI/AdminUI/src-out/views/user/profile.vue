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
          账户：<span class="info">{{ userInfo.Account }}</span>
        </div>
        <div class="text item">
          名称：<span class="info">{{ userInfo.Name }}</span>
        </div>
        <div class="text item">
          电话：<span class="info">{{ userInfo.Phone }}</span>
        </div>
        <div class="text item">
          邮箱：<span class="info">{{ userInfo.Email }}</span>
        </div>
        <div class="text item">
          机构：<span class="info">{{ userInfo.OrgnizationName }}</span>
        </div>
        <div class="text item">
          角色：<span class="info">{{ userInfo.RolesName }}</span>
        </div>
      </el-card>
    </el-main>
    <!-- 编辑窗口 -->
    <el-dialog
      title="信息编辑"
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
        :inline="true"
        class="demo-ruleForm"
      >
        <el-form-item prop="Name" label="名称" required>
          <el-input type="text" v-model:value="eidtModel.Name"></el-input>
        </el-form-item>
        <el-form-item prop="Email" label="邮箱" required>
          <el-input type="text" v-model:value="eidtModel.Email"></el-input>
        </el-form-item>
        <el-form-item prop="Contract" label="联系人">
          <el-input type="text" v-model:value="eidtModel.Contract"></el-input>
        </el-form-item>
        <el-form-item prop="Phone" label="电话">
          <el-input type="text" v-model:value="eidtModel.Phone"></el-input>
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

<script>
import { dalUser } from '@/api/sys'
import { mapGetters } from 'vuex'
export default {
  name: 'userprofile',
  computed: {
    ...mapGetters(['userInfo']),
  },
  data() {
    return {
      eidtModel: { Name: '', Email: '', Contract: '', Phone: '' },
      showEditForm: false, //是否显示编辑窗
      formLabelWidth: '120px',
      rules: {
        Name: [{ required: true, message: '名称必填', trigger: 'change' }],
        Email: [{ required: true, message: '邮箱必填', trigger: 'change' }],
      },
    }
  },

  methods: {
    edit() {
      //编辑
      let me = this
      for (const key in me.eidtModel) {
        if (me.userInfo.hasOwnProperty(key)) {
          me.eidtModel[key] = me.userInfo[key]
        }
      }
      this.showEditForm = true
    },
    saveData() {
      //保存数据
      let me = this

      this.showEditForm = true
      this.$refs['editDataForm'].validate((valid) => {
        if (valid) {
          saveInfo(me.userInfo.Id, me.eidtModel).then((resp) => {
            if (resp.success) {
              me.$message('保存成功！')
              store.dispatch('user/getInfo')
              this.showEditForm = false
            }
          })
        } else {
          me.$message({
            message: '请完善必填项后再继续',
            type: 'warning',
          })
        }
      })
    },
  },
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
