import IdentityProviderTemplateService from "@/services/identity-provider-template.service"
import { Effect, Reducer } from "umi";


export interface IdpTemplatesModelState {
    templates?: []
}

interface IdpTemplatesModelType {
    namespace: 'idpTemplates',
    state: IdpTemplatesModelState,
    effects: {
        fetch: Effect
    },
    reducers: {
        save: Reducer<IdpTemplatesModelState>,
        clear: Reducer<IdpTemplatesModelState>
    }
}

const Model: IdpTemplatesModelType = {
    namespace: 'idpTemplates',
    state: {},
    effects: {
        *fetch({ payload }, { call, put }): any {
            const data = yield call(IdentityProviderTemplateService.getAll, { ...payload })
            yield put({
                type: 'save',
                payload: {
                    templates: data
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

export default Model;