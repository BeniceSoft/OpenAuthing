import { RequestConfig, AxiosResponse, RequestOptions } from 'umi';
import { ResponseResult } from "@/@types";
import { initialTheme } from './lib/misc';
import InitialStateModel from './@types/InitialStateModel';
import { toast } from 'react-hot-toast';
import ReactDOM from 'react-dom';
import { getOidc } from './lib/utils';


export const request: RequestConfig = {
    timeout: 10000,
    headers: { 'X-Requested-With': 'XMLHttpRequest' },
    // other axios options you want
    errorConfig: {
        // 错误抛出
        errorThrower: (res: ResponseResult) => {
            const { success, data, errorCode, errorMessage } = res;
            if (!success) {
                const error: any = new Error(errorMessage);
                error.name = 'BizError';
                error.info = { errorCode, errorMessage, data };
                throw error; // 抛出自制的错误
            }
        },
        // 错误接收及处理
        errorHandler: (error: any, opts: any) => {
            console.error(error)
            if (opts?.skipErrorHandler) throw error;
            // 我们的 errorThrower 抛出的错误。
            if (error.name === 'BizError') {
                const errorInfo: ResponseResult | undefined = error.info;
                if (errorInfo) {
                    const { errorCode, errorMessage } = errorInfo;
                    if (errorCode === 401) {
                        toast.error('登录状态已失效，正在跳转到登录...')
                        getOidc().loginAsync()
                        return;
                    }
                    toast.error(errorMessage);
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
    requestInterceptors: [
        (config: RequestOptions) => {
            const oidc = getOidc()
            let headers = config.headers || {}
            if (oidc.tokens) {
                headers['Authorization'] = `Bearer ${oidc.tokens.accessToken}`
            }

            let baseURL: string | null = AM_ADMIN_API_BASE_URL
            // if (process.env.NODE_ENV === 'development') {
            //     baseURL = null
            // }
            const newConfig = {
                ...config,
                baseURL,
                headers
            }

            return newConfig
        }
    ],
    responseInterceptors: [
        (response: AxiosResponse) => {
            const { code: errorCode, message: errorMessage, data } = response.data;
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
    const theme = initialTheme() ?? ''

    return ({
        theme
    } as InitialStateModel)
}

export const qiankun = {
    /**
    * bootstrap 只会在微应用初始化的时候调用一次，下次微应用重新进入时会直接调用 mount 钩子，不会再重复触发 bootstrap。
    * 通常我们可以在这里做一些全局变量的初始化，比如不会在 unmount 阶段被销毁的应用级别的缓存等。
    */
    async bootstrap(props: any) {
        console.log('am-admin bootstrap', props);
    },

    /**
     * 应用每次进入都会调用 mount 方法，通常我们在这里触发应用的渲染方法
     */
    async mount(props: any) {
        console.log('am-admin mount', props);
        window.qiankun = { ...props }

        // ReactDOM.render(<App />, props.container ? props.container.querySelector('#root') : document.getElementById('root'));
    },
    /**
     * 应用每次 切出/卸载 会调用的方法，通常在这里我们会卸载微应用的应用实例
     */
    async unmount(props: any) {
        console.log('am-admin unmount', props);

        ReactDOM.unmountComponentAtNode(
            props.container ? props.container.querySelector('#root') : document.getElementById('root'),
        );
    },

    /**
     * 可选生命周期钩子，仅使用 loadMicroApp 方式加载微应用时生效
     */
    async update(props: any) {
        console.log('update props', props);
    }
};