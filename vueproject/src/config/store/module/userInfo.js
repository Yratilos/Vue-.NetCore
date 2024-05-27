const userInfo = {
    namespaced: true,
    state() {
        return {
            // 定义状态
            id: "",
            name: ""
        }
    },
    mutations: {
        // 定义修改状态的方法
        updateUser(state, user) {
            state.id = user.id;
            state.name = user.name;
        }
    },
    actions: {
        // 定义触发mutations的方法
    },
    getters: {
        // 定义获取state的计算属性
    }
}
export default userInfo