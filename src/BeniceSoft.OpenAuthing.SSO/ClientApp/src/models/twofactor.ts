import { Reducer, Effect, history } from 'umi';
import AuthService from '@/services/auth.service'

export interface TwoFactorModelState {
    authenticatorUri?: string,
    hasAuthenticator?: boolean
    is2FaEnabled?: boolean
    isMachineRemembered?: boolean
    recoveryCodesLeft?: number
}

export interface TwoFactorModelType {
    namespace: 'twofactor';
    state: TwoFactorModelState;
    effects: {
        fetchTwoFactorState: Effect
        disableTwoFactor: Effect
        fetchAuthenticatorUri: Effect
        enableAuthenticator: Effect
    };
    reducers: {
        save: Reducer<TwoFactorModelState>;
    };
}

const Model: TwoFactorModelType = {
    namespace: 'twofactor',
    state: {
    },
    effects: {
        *fetchTwoFactorState(_, { call, put }): any {
            const data = yield call(AuthService.getTwoFactorState)
            yield put({
                type: 'save',
                payload: {
                    ...data
                }
            })
        },
        *disableTwoFactor(_, { call, put }): any {
            yield call(AuthService.disableTwoFactor)

            yield put({
                type: 'fetchTwoFactorState'
            })
        },
        *fetchAuthenticatorUri(_, { call, put }): any {
            const authenticatorUri = yield call(AuthService.generateAuthenticatorUri)
            yield put({
                type: 'save',
                payload: {
                    authenticatorUri
                }
            })
        },
        *enableAuthenticator({ payload }, { call }): any {
            const { code } = payload
            const data = yield call(AuthService.enableAuthenticator, code)
            if (data) {
                const { recoveryCodes } = data
                if (recoveryCodes) {
                    history.push({
                        pathname: '/settings/2fa/show-recovery-codes'
                    }, {
                        recoveryCodes
                    })
                }
            }
        }
    },
    reducers: {
        save(state: any, action: any) {
            return {
                ...state,
                ...action.payload,
            };
        },
    }
}

export default Model;