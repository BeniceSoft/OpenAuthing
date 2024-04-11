export interface UserInfo {
    id: string
    userName: string
    nickname: string
    avatar?: string
    enabled: boolean
    phoneNumber: string
    emailAddress: string
    isSystemBuiltIn: boolean
}


export interface UserProfile {
    id: string
    userName: string
    nickname: string
    avatar?: string
    phoneNumber: string
    emailAddress: string
    creationTime: Date
}

export type ResetPasswordValidationMethod = 'email' | 'phone'

export interface ForgotPasswordReq {
    email?: string
    phoneNumber?: string
    validationMethod: ResetPasswordValidationMethod
}

export interface ResetPasswordReq {
    uid?: string
    password?: string
    confirmPassword?: string
    code?: string
    validationMethod: ResetPasswordValidationMethod
}