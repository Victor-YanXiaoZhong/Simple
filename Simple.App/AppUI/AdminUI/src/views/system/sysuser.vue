<template>
  <el-container>
    <el-main>
      <el-row style="padding: 10px">
        <el-button-group>
          <el-button
            type="primary"
            icon="el-icon-edit"
            @click="add"
            size="medium"
            >新增</el-button
          >
        </el-button-group>
      </el-row>
      <el-table
        :data="datas"
        border
        size="mini"
        :highlight-current-row="true"
        style="width: 100%; margin-top: 10px"
      >
        <el-table-column
          prop="Account"
          label="账户"
          width="160"
        ></el-table-column>
        <el-table-column prop="Name" label="名称" width="160"></el-table-column>
        <el-table-column
          prop="Email"
          label="邮箱"
          width="160"
        ></el-table-column>
        <el-table-column
          prop="Contract"
          label="联系人"
          width="160"
        ></el-table-column>
        <el-table-column
          prop="Phone"
          label="电话"
          width="160"
        ></el-table-column>
        <el-table-column
          prop="SysRole.Name"
          label="用户角色"
          width="160"
        ></el-table-column>
        <el-table-column
          prop="RegionName"
          label="数据区域"
          width="160"
        ></el-table-column>
        <el-table-column fixed="right" label="操作" align="center" width="150">
          <template slot-scope="scope">
            <el-button
              @click="SetFlag(scope.row)"
              type="text"
              style="color: #e6a23c"
              size="small"
              >{{ scope.row.IsDeleted ? "启用" : "停用" }}</el-button
            >
            <el-button type="text" size="small" @click="edit(scope.row)"
              >编辑</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-main>
    <!-- 编辑窗口 -->
    <el-dialog
      title="数据编辑"
      :visible.sync="showEditForm"
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
        <el-form-item prop="SysRoleId" label="角色信息">
          <el-select v-model="eidtModel.SysRoleId" placeholder="请选择角色">
            <el-option
              v-for="item in roles"
              :key="item.Id"
              :label="item.Name"
              :value="item.Id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item prop="" label="数据区域">
          <el-cascader
            class="region-select"
            style="width: 170px"
            size="large"
            :options="regionDataPlus"
            v-model="selectedRegion"
            @change="selectRegion"
          >
          </el-cascader>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="cancelEdit">取 消</el-button>
        <el-button type="primary" @click="saveData">确 定</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>

<script>
import { qUser, dalUser, qRole } from "@/api/sys";
import { sysroles } from "@/api/sys";
import { mapGetters } from "vuex";
export default {
  name: "sysUser",
  computed: {
    ...mapGetters(["info"]),
  },
  data() {
    return {
      regionDataPlus, //省市区三级联动数据（带“全部”选项）
      selectedRegion: [""], //选中的区域
      datas: [],
      eidtModel: {
        Name: "",
        Email: "",
        Contract: "",
        Phone: "",
        SysRoleId: 0,
        RegionCode: "",
        RegionName: "全部",
      },
      currentData: {}, //当前操作对象
      showEditForm: false, //是否显示编辑窗
      formLabelWidth: "120px",
      rules: {
        Name: [{ required: true, message: "名称必填", trigger: "change" }],
        Email: [{ required: true, message: "邮箱必填", trigger: "change" }],
      },
      roles: [], //系统角色信息
    };
  },
  mounted() {
    this.getDatas();
  },
  methods: {
    selectRegion(e) {
      //区域选择后的处理事件
      let code = "",
        name = "";
      for (let index = 0; index < e.length; index++) {
        const element = e[index];
        name += CodeToText[element];
      }
      code = e.join("|");
      let me = this;
      me.eidtModel.RegionCode = code;
      me.eidtModel.RegionName = name;
    },
    add() {
      //新增
      this.eidtModel = {
        Name: "",
        Email: "",
        Contract: "",
        Phone: "",
        SysRoleId: "",
        RegionCode: "",
        RegionName: "全部",
      };
      this.showEditForm = true;
    },
    edit(model) {
      //编辑
      let me = this;
      this.currentData = model;
      for (const key in me.eidtModel) {
        if (model.hasOwnProperty(key)) {
          me.eidtModel[key] = this.currentData[key];
        }
      }
      if (this.currentData.RegionCode != "") {
        me.selectedRegion = this.currentData.RegionCode.split("|");
      }
      this.showEditForm = true;
    },
    cancelEdit() {
      this.showEditForm = false;
      this.getDatas();
    },
    saveData() {
      //保存数据
      let me = this;

      this.showEditForm = true;
      this.$refs["editDataForm"].validate((valid) => {
        if (valid) {
          if (me.currentData.Id > 0) {
            //新增
            dalUser("edit", me.currentData.Id, me.eidtModel).then((resp) => {
              if (resp.success) {
                me.$message("保存成功！");
                me.getDatas();
                me.cancelEdit();
              }
            });
          } else {
            me.eidtModel.Account = me.eidtModel.Email;
            me.eidtModel.SysOrgnizationId = me.userInfo.OrgnizationId;

            //新增
            dalUser("add", me.eidtModel).then((resp) => {
              if (resp.success) {
                me.$message("保存成功！");
                me.getDatas();
                me.cancelEdit();
              }
            });
          }
        } else {
          me.$message({
            message: "请完善必填项后再继续",
            type: "warning",
          });
        }
      });
    },
    SetFlag(model) {
      //设置停用启用的标量值
      let me = this;
      this.$confirm(
        `确定要${model.IsDeleted ? "启用" : "停用"}此记录吗？`,
        "询问",
        {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        }
      )
        .then(() => {
          dalUser("setFlag?id=" + model.Id, model.IsDeleted ? 0 : 1)
            .then((resp) => {
              if (resp.success) {
                model.IsDeleted = !model.IsDeleted;
                me.$message({
                  type: "success",
                  message: "设置成功!",
                });
              }
            })
            .catch((err) => {
              me.$message({
                type: "waring",
                message: "设置没有成功!",
              });
            });
        })
        .catch(() => {});
    },
    getDatas() {
      let me = this;
      qUser(list, `IsTenant=false`).then((resp) => {
        me.datas = resp.data;
      });
      qRole("list").then((resp) => {
        me.roles = resp.data;
      });
    },
  },
};
</script>
