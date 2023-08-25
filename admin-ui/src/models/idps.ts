import { ExternalIdentityProvider } from "@/@types/identityProvider";
import IdentityProviderService from "@/services/identity-provider.service";
import { Effect, Reducer } from "umi";

export interface IdPsModelState {
    idps?: ExternalIdentityProvider[]
}

interface IdPsModelType {
    namespace: 'idps',
    state: IdPsModelState,
    effects: {
        fetch: Effect
    },
    reducers: {
        save: Reducer<IdPsModelState>,
        clear: Reducer<IdPsModelState>
    }
}

const Model: IdPsModelType = {
    namespace: 'idps',
    state: {},
    effects: {
        *fetch({ payload }, { call, put }): any {
            const data = yield call(IdentityProviderService.getAll, { ...payload })
            yield put({
                type: 'save',
                payload: {
                    idps: data
                }
            })
        }
    },
    reducers: {
        save(state, { payload }) {
            return {
                ...state,
                ...payload
            }
        },
        clear() {
            return {}
        }
    }
}

export default Model