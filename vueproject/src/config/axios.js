import axios from 'axios'

axios.defaults.baseURL = '/api'

// �������������
axios.interceptors.request.use(function (config) {
    // �ڷ�������֮ǰ��Щʲô
    return config;
}, function (error) {
    // �����������Щʲô
    return Promise.reject(error);
});

// �����Ӧ������
axios.interceptors.response.use(function (response) {
    // 2xx ��Χ�ڵ�״̬�붼�ᴥ���ú�����
    const code = response.status
    if (code === 200) {
        const res = response.data
        if (!res.Status) {
            return res.data
        }
    }
    return Promise.reject(error);
}, function (error) {
    // ���� 2xx ��Χ��״̬�붼�ᴥ���ú�����
    // ����Ӧ��������ʲô
    return Promise.reject(error);
});

export default axios;