import {RequestConfig, AxiosResponse, getDvaApp} from 'umi';
import AuthService from '@/services/auth.service'
import {toast} from 'react-hot-toast';
import {ResponseResult} from '@/@types';

export const request: RequestConfig = {
    timeout: 10000,
    headers: {'X-Requested-With': 'XMLHttpRequest'},
    // other axios options you want
    errorConfig: {
        // 错误抛出
        errorThrower: (res: ResponseResult) => {
            const {success, data, errorCode, errorMessage} = res;
            console.log('success: ', success)
            if (!success) {
                const error: any = new Error(errorMessage);
                error.name = 'BizError';
                error.info = {errorCode, errorMessage, data};
                throw error; // 抛出自制的错误
            }
        },
        // 错误接收及处理
        errorHandler: (error: any, opts: any) => {
            if (opts?.skipErrorHandler) throw error;
            // 我们的 errorThrower 抛出的错误。
            if (error.name === 'BizError') {
                const errorInfo: ResponseResult | undefined = error.info;
                if (errorInfo) {
                    const {errorCode, errorMessage} = errorInfo;
                    if (errorCode === 401) {
                        toast.error('登录状态已失效，正在跳转到登录...')
                        getDvaApp()._store.dispatch({
                            type: 'login/logout'
                        })
                        return;
                    }
                    errorMessage && toast.error(errorMessage);
                }
            } else if (error.response) {
                // Axios 的错误
                // 请求成功发出且服务器也响应了状态码，但状态代码超出了 2xx 的范围
                toast.error(`Response status:${error.response.status}`);
            } else if (error.request) {
                // 请求已经成功发起，但没有收到响应
                // \`error.request\` 在浏览器中是 XMLHttpRequest 的实例，
                // 而在node.js中是 http.ClientRequest 的实例
                toast.error('None response! Please retry.');
            } else {
                // 发送请求时出了点问题
                toast.error('Request error, please retry.');
            }
        },
    },
    requestInterceptors: [],
    responseInterceptors: [
        (response: AxiosResponse) => {

            const {code: errorCode, message: errorMessage, data} = response.data;
            response.data = {
                errorCode,
                errorMessage,
                data,
                success: errorCode === 200,
            };
            return response;
        },
    ]
};

export async function getInitialState() {
    const user = await AuthService.getUser()
    const isAuthenticated = user !== null

    return ({
        isAuthenticated,
        currentUser: user
    })
}