import { defineStore } from 'pinia'
const userInfo = defineStore("user",{
    namespaced: true,
    persist: true,
    state: () => ({
        id: "",
        name: ""
    }),
    actions: {
        // 定义触发actions的方法
        updateUser(user) {
            this.id = user.id;
            this.name = user.name;
        }
    },
    getters: {
        // 定义获取state的计算属性
    },
})
export default userInfo