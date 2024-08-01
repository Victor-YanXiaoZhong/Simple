<template>
  <el-tag v-for="tag in tags" :key="tag.name" closable :type="tag.type">
      {{ tag.name }}
    </el-tag>
    <el-result
        icon="success"
        title="操作成功"
        sub-title="这是子介绍"
      >
        <template #extra>
          <el-button type="primary">返回</el-button>
        </template>
      </el-result>
</template>

<script setup>
import { ref } from 'vue'
const tags = ref([
  { name: 'Tag 1', type: 'primary' },
  { name: 'Tag 2', type: 'success' },
  { name: 'Tag 3', type: 'info' },
  { name: 'Tag 4', type: 'warning' },
  { name: 'Tag 5', type: 'danger' },
])
</script>

<style>

</style>