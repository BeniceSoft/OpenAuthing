import { request } from '@/lib/request'
import { CurrentUserInfo, LoginWith2FaModel, LoginWithPasswordModel, LoginWithRecoveryCode } from "@/@types/auth";
import { ResponseResultWithT } from '@/@types';

const UserStoreName = '__OA_USERINFO'
const API_ROOT = "/api/account";

class AuthService {

    public isAuthenticated(): boolean {
        return !!service.getUser()
    }

    public async getExternalIdPs() {
        const { data } = await request(API_ROOT + '/getexternalloginproviders')
        return data ?? []
    }

    public async logout() {
        if (service.isAuthenticated()) {
            await request('/connect/logout')
        }

        localStorage.removeItem(UserStoreName)
    }

    public async login(model: LoginWithPasswordModel) {
        const response = await request(API_ROOT + '/login', {
            method: 'POST',
            data: model
        })
        const { data } = response

        if (data) {
            const { userInfo } = data
            userInfo && service.storeUser(userInfo)
        }

        return response
    }

    public async loginWith2Fa(model: LoginWith2FaModel) {
        const response = await request(API_ROOT + '/loginwith2fa', {
            method: 'POST',
            data: model
        })

        const { data } = response

        if (data) {
            const { userInfo } = data
            userInfo && service.storeUser(userInfo)
        }

        return response
    }

    public async loginWithRecoveryCode(model: LoginWithRecoveryCode) {
        const { data } = await request(API_ROOT + '/loginWithRecoveryCode', {
            method: 'POST',
            data: model
        })

        if (data) {
            const { userInfo } = data
            userInfo && service.storeUser(userInfo)
        }

        return data
    }

    public async getUser(fromCache: boolean = true): Promise<CurrentUserInfo | null> {
        try {
            if (fromCache) {
                const json = localStorage.getItem(UserStoreName)
                console.log(UserStoreName, json)
                if (json) return JSON.parse(json)

            } else {

                const { data } = await request(API_ROOT + '/me')
                this.storeUser(data)

                return data
            }
        } catch (e) {
            console.error(e)
        }

        return null
    }

    public storeUser(user: any) {
        localStorage.setItem(UserStoreName, JSON.stringify(user))
    }

    public async generateAuthenticatorUri(): Promise<string> {
        const { data } = await request(API_ROOT + '/generateAuthenticatorUri')

        if (data) {
            const { authenticatorUri } = data
            return authenticatorUri
        }

        return ''
    }

    public async enableAuthenticator(code: string) {
        return await request(API_ROOT + '/enableAuthenticator', {
            method: 'POST',
            data: {
                code
            }
        })
    }

    public async getTwoFactorState() {
        return await request(API_ROOT + '/towFactorAuthentication')
    }

    public async disableTwoFactor() {
        return await request(API_ROOT + '/disable2Fa', { method: 'POST' })
    }

    public async getRecoveryCodes(): Promise<ResponseResultWithT<string[] | null>> {
        return await request(API_ROOT + '/getRecoveryCodes', { method: 'GET' })
    }
}

const service = new AuthService();

export default service;