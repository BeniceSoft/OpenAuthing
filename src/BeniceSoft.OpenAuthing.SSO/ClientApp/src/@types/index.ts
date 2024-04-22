export interface ResponseResultWithT<TData> {
    success: boolean
    code: number
    errorMessage?: string,
    data?: TData
}

export interface ResponseResult extends ResponseResultWithT<any> { }