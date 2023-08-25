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

export interface UserDetail extends UserInfo {

}