import { RoleInfo } from "@/@types/role"
import RoleService from "@/services/role.service"
import { Effect, Reducer } from "umi"

export interface RolesModelState {
    totalCount?: number
    items?: RoleInfo[]
}

interface RolesModelType {
    namespace: 'roles'
    state: RolesModelState
    effects: {
        fetch: Effect
    }
    reducers: {
        save: Reducer<RolesModelState>,
        clear: Reducer<RolesModelState>
    }
}

const Model: RolesModelType = {
    namespace: 'roles',
    state: {},
    effects: {
        *fetch({ payload }, { call, put }): any {
            const data = yield call(RoleService.getAll, { ...payload })
            yield put({
                type: 'save',
                payload: {
                    ...data
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