// 自动路由生成器
const routeFiles = import.meta.glob('../views/**/*.vue');
const layoutComponent = () => import('../layout/AppLayout.vue');

function generateRoute(files) {
  const routes = [];

  for (const path in files) {
    const pathParts = path.split('/').slice(2); // 移除 '../views/'
    let currentLevel = routes; // 开始于根路由层级

    // 遍历除了文件名以外的路径部分
    for (let i = 0; i < pathParts.length - 1; i++) {
      const part = '/' + pathParts[i].toLowerCase();
      // 查找或创建当前路径部分的路由
      let childRoute = currentLevel.find(route => route.path === part || (route.path === '/' && part === 'index'));
      if (!childRoute) {
        childRoute = {
          name:  part.replace('/', '').replace('-', '_'),
          path:  part,
          children: [],
          component: i === 0 ? layoutComponent : null // 只有第一层才使用 AppLayout
        };
        currentLevel.push(childRoute);
      }
      currentLevel = childRoute.children; // 进入下一层级
    }

    // 添加文件对应的路由
    const fileName = pathParts[pathParts.length - 1];
    const name = fileName.replace(/\.\w+$/, '').toLowerCase();
    let tmpPath = joinPath(pathParts)
    currentLevel.push({
      name:  tmpPath,
      path: name === 'index' ? tmpPath: tmpPath,
      component: files[path] // 动态导入组件
    });
  }

  return routes;
}

function clearFirstSlash(path) {
  return path.replace(/^\//, '');
}

function joinPath(pathParts){
  let tmp = '';
  for (let index = 0; index < pathParts.length; index++) {
    const element = pathParts[index];
    tmp+= '/' + element.replace(/\.\w+$/, '').toLowerCase();
  }
  return tmp
}
const generaroutes = generateRoute(routeFiles);
export default generaroutes;
