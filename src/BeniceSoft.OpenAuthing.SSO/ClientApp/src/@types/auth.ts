export interface ExternalLoginProvider {
    name: string;
    displayName: string;
    providerName: string
}

export interface LoginModelBase {
    returnUrl?: string
    rememberMe: boolean
}

export interface LoginWithPasswordModel extends LoginModelBase {
    userName: string
    password: string
}

export interface LoginWith2FaModel extends LoginModelBase {
    twoFactorCode: string
    rememberMachine: boolean
}

export interface LoginWithRecoveryCode extends LoginModelBase {
    recoveryCode: string
}

export interface CurrentUserInfo {
    id: string
    userName: string
    nickname: string
    avatar?: string
}