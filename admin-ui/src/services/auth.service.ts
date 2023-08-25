import { request } from '@/lib/request'
import { CurrentUserInfo, LoginWith2FaModel, LoginWithPasswordModel, LoginWithRecoveryCode } from "@/@types/auth";

const UserStoreName = '.AM.User'

class AuthService {

    public isAuthenticated(): boolean {
        return !!this.getUser()
    }

    public async getExternalIdPs() {
        const { data } = await request('/api/account/getexternalloginproviders')
        return data ?? []
    }

    public logout() {
        if (this.isAuthenticated()) {
            // todo call logout endpoint
        }

        localStorage.removeItem(UserStoreName)
    }

    public async login(model: LoginWithPasswordModel) {
        const { data } = await request('/api/account/login', {
            method: 'POST',
            data: model
        })

        if (data) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return data
    }

    public async loginWith2Fa(model: LoginWith2FaModel) {
        const { data } = await request('/api/account/loginwith2fa', {
            method: 'POST',
            data: model
        })

        if (data) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return data
    }

    public async loginWithRecoveryCode(model: LoginWithRecoveryCode) {
        const { data } = await request('/api/account/loginWithRecoveryCode', {
            method: 'POST',
            data: model
        })

        if (data) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return data
    }

    public async getUser(fromCache: boolean = true): Promise<CurrentUserInfo | undefined> {
        
        try {
            if (fromCache) {
                const json = localStorage.getItem(UserStoreName)
                console.log(UserStoreName, json)
                if (json) return JSON.parse(json)

            } else {

                const { data } = await request('/api/account/me')
                this.storeUser(data)

                return data
            }
        } catch (e) {
            console.error(e)
        }

        return undefined
    }

    public storeUser(user: any) {
        localStorage.setItem(UserStoreName, JSON.stringify(user))
    }

    public async generateAuthenticatorUri(): Promise<string> {
        const { data } = await request('/api/account/generateAuthenticatorUri')

        if (data) {
            const { authenticatorUri } = data
            return authenticatorUri
        }

        return ''
    }

    public async enableAuthenticator(code: string) {
        const { data } = await request('/api/account/enableAuthenticator', {
            method: 'POST',
            data: {
                code
            }
        })

        return data
    }

    public async getTwoFactorState() {
        const { data } = await request('/api/account/towFactorAuthentication')

        return data
    }

    public async disableTwoFactor() {
        const data = await request('/api/account/disable2Fa', { method: 'POST' })
        console.log(data)
    }
}

const service = new AuthService();

export default service;