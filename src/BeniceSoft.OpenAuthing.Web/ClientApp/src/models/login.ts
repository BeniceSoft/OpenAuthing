import { Reducer, Effect, history, useModel } from 'umi';
import AuthService from '@/services/auth.service'
import { ExternalLoginProvider } from "@/@types/auth";
import { toast } from 'react-hot-toast';

export interface LoginModelState {
    externalLoginProviders?: ExternalLoginProvider,
}

export interface LoginModelType {
    namespace: 'login';
    state: LoginModelState;
    effects: {
        fetchLoginProviders: Effect,
        login: Effect,
        loginWith2Fa: Effect
        logout: Effect
        loginWithRecoveryCode: Effect
    };
    reducers: {
        save: Reducer<LoginModelState>;
    };
}

const Model: LoginModelType = {
    namespace: 'login',
    state: {
    },
    effects: {
        *logout(_, { call, put }) {
            yield call(AuthService.logout)
            yield put({
                type: 'save',
                payload: {
                    isAuthenticated: false,
                    currentUser: undefined
                }
            })

            console.log('跳转登录')
            history.push({
                pathname: '/account/login',
                search: `?returnUrl=${encodeURIComponent(history.location.pathname)}`
            })
        },
        *fetchLoginProviders({ }, { put, call }): any {
            const externalLoginProviders = yield call(AuthService.getExternalIdPs)
            yield put({
                type: 'save',
                payload: {
                    externalLoginProviders
                }
            })
        },
        *login({ payload }, { call, put }): any {
            const data: any = yield call(AuthService.login, payload)
            if (data) {
                const { requiresTwoFactor, returnUrl, userInfo } = data
                if (requiresTwoFactor) {
                    history.push({
                        pathname: '/account/loginwith2fa',
                        search: `?returnUrl=${encodeURIComponent(returnUrl)}`
                    })
                    return
                }

                // Message.success('登录成功，正在跳转...', 1000)
                toast.success('登录成功，正在跳转...')
                window.location = returnUrl ?? '/'
            }
        },
        *loginWith2Fa({ payload }, { call, put }): any {
            const data: any = yield call(AuthService.loginWith2Fa, payload)
            if (data) {
                const { returnUrl } = data

                toast.success('登录成功，正在跳转...')
                window.location = returnUrl ?? '/'
            }
        },
        *loginWithRecoveryCode({ payload }, { put, call }): any {
            const data: any = yield call(AuthService.loginWithRecoveryCode, payload)
            if (data) {
                const { returnUrl } = data

                toast.success('登录成功，正在跳转...')
                window.location = returnUrl ?? '/'
            }
        }
    },
    reducers: {
        save(state: any, action: any) {
            return {
                ...state,
                ...action.payload,
            };
        }
    }
}

export default Model;