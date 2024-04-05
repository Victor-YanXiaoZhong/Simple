const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  lintOnSave:false,
  css: {
    //requireModuleExtension: false
  },
  devServer:{
    port:8080
  }
})
