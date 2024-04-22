import { ResponseResult, ResponseResultWithT } from '@/@types';
import { error } from 'console'
import { request as umiRequest, Request } from 'umi'

let controller = new AbortController();

// 因为 umi-request 在 errorHandler 处理之后还是会抛出异常，故这里包装一下
// 详细可以看 https://github.com/umijs/umi/issues/8519
export const request = <T>(
    url: string,
    opts: any = { method: 'GET', withCredentials: true, signal: controller.signal },
) => new Promise<ResponseResultWithT<T>>(async (resolve, reject) => {
    try {
        const response: any = await umiRequest<any>(url, opts);

        console.log('response', response)
        const { errorCode, errorMessage, data } = response

        resolve({ success: true, code: errorCode, errorMessage, data })
    } catch (error) {
        console.error('++++request error: ', error)
        reject()
    } finally {

    }
    // .then((data) => {
    //     console.log("result: ", data)

    //     return { success: true, ...data }
    // })
    // .catch(({ code, status, ...error }) => {
    //     console.error('++++request error: ', error)
    //     try {
    //         if (code === 'ERR_CANCELED') return
    //         return { success: false, errorCode: status, ...error }
    //     } finally {
    //         controller.abort()

    //         // 重新创建一个，否则后续的 useRequest 不会执行
    //         controller = new AbortController();
    //     }
    // })
})
