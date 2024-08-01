import { defineStore } from "pinia"
 
const useUserInfoStore = defineStore('userInfo', {
  state: () => ({Name:"",Account:"",Id:0,Avatar:"",Phone:"",SupperAdmin:false,SysOrgnizationId:0,Token:"",LogTime:"",AdminOrg:false}
  ),
})
export default useUserInfoStore