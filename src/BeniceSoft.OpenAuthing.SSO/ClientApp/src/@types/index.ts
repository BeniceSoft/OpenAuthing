export interface ResponseResultWithT<TData> {
    success: boolean
    errorCode: number
    errorMessage: string,
    data?: TData
}

export interface ResponseResult extends ResponseResultWithT<any> { }