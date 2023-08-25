import { idPTemplate } from "@/@types/identityProviderTemplate"
import IdentityProviderTemplateService from "@/services/identity-provider-template.service"
import { Effect, Reducer } from "umi"

export interface CreateIdPModelState {
    template?: idPTemplate
}

interface CreateIdPModelType {
    namespace: 'createIdP',
    state: CreateIdPModelState,
    effects: {
        fetch: Effect
    },
    reducers: {
        save: Reducer<CreateIdPModelState>,
        clear: Reducer<CreateIdPModelState>
    }
}

const Model: CreateIdPModelType = {
    namespace: "createIdP",
    state: {},
    effects: {
        *fetch({ payload }, { call, put }): any {
            const { providerName } = payload
            const data = yield call(IdentityProviderTemplateService.get, providerName)
            yield put({
                type: 'save',
                payload: {
                    template: data
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