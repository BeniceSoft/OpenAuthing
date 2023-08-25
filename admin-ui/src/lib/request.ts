import { request as umiRequest, Request } from 'umi'

// 因为 umi-request 在 errorHandler 处理之后还是会抛出异常，故这里包装一下
// 详细可以看 https://github.com/umijs/umi/issues/8519
export const request: Request = (
    url: string,
    opts: any = { method: 'GET', withCredentials: false }
) => {
    return umiRequest(url, opts)
        .then((data) => ({ success: true, ...data }))
        .catch((error) => ({ success: false, ...error }))
}