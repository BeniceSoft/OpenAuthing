import { history } from 'umi'
import { request } from '@/lib/request'
import { CurrentUserInfo, LoginWith2FaModel, LoginWithPasswordModel, LoginWithRecoveryCode } from "@/@types/auth";
import { ResponseResultWithT } from '@/@types';

export const UserStoreName = '__OA_USERINFO'
const API_ROOT = "/api/account";

class AuthService {

    public isAuthenticated = async (): Promise<boolean> => {
        const user = await this.getUser();
        return user !== null && user !== undefined;
    }

    public getExternalIdPs = async (): Promise<ResponseResultWithT<any>> => {
        const { data } = await request(API_ROOT + '/getexternalloginproviders')
        return data ?? []
    }

    public logout = async (returnUrl?: string): Promise<void> => {
        if (await this.isAuthenticated()) {
            await request('/connect/logout')
        }

        localStorage.removeItem(UserStoreName)
    }

    public login = async (model: LoginWithPasswordModel): Promise<ResponseResultWithT<any>> => {
        const response = await request(API_ROOT + '/login', {
            method: 'POST',
            data: model
        })
        const { data } = response

        if (data && data.loginSuccess) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return response
    }

    loginWith2Fa = async (model: LoginWith2FaModel) => {
        const response = await request(API_ROOT + '/loginwith2fa', {
            method: 'POST',
            data: model
        })

        const { data } = response

        if (data) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return response
    }

    loginWithRecoveryCode = async (model: LoginWithRecoveryCode) => {
        const response = await request(API_ROOT + '/loginWithRecoveryCode', {
            method: 'POST',
            data: model
        })
        const { data } = response;

        if (data) {
            const { userInfo } = data
            userInfo && this.storeUser(userInfo)
        }

        return response
    }

    getUser = async (fromCache: boolean = true): Promise<CurrentUserInfo | null> => {
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

    storeUser = (user: any) => {
        localStorage.setItem(UserStoreName, JSON.stringify(user))
    }

    generateAuthenticatorUri = async (): Promise<string> => {
        const { data } = await request(API_ROOT + '/generateAuthenticatorUri')

        if (data) {
            const { authenticatorUri } = data
            return authenticatorUri
        }

        return ''
    }

    enableAuthenticator = async (code: string) => {
        return await request(API_ROOT + '/enableAuthenticator', {
            method: 'POST',
            data: {
                code
            }
        })
    }

    getTwoFactorState = async () => {
        return await request(API_ROOT + '/towFactorAuthentication')
    }

    disableTwoFactor = async () => {
        return await request(API_ROOT + '/disable2Fa', { method: 'POST' })
    }

    getRecoveryCodes = async (): Promise<ResponseResultWithT<string[] | null>> => {
        return await request(API_ROOT + '/getRecoveryCodes', { method: 'GET' })
    }
}


export default new AuthService()