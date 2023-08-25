import { UserInfo } from "@/@types/user"
import UserService from "@/services/user.service"
import { toast } from "react-hot-toast"
import { Effect, Reducer } from "umi"

export interface UsersModelState {
    totalCount?: number
    users?: UserInfo[]
}

interface UsersModelType {
    namespace: 'users',
    state: UsersModelState,
    effects: {
        fetch: Effect,
        create: Effect
    },
    reducers: {
        save: Reducer<UsersModelState>,
        clear: Reducer<UsersModelState>
    }
}

const Model: UsersModelType = {
    namespace: 'users',
    state: {},
    effects: {
        *fetch({ payload }, { call, put }): any {
            const data = yield call(UserService.getAll, { ...payload })
            const { totalCount = 0, items = [] } = data;
            yield put({
                type: 'save',
                payload: {
                    totalCount,
                    users: items
                }
            })
        },
        *create({ payload }, { call, put }): any {
            const { pageIndex, pageSize, input } = payload

            const result = yield call(UserService.create, { ...input });

            if (result) {
                toast.success('用户创建成功')

                yield put({
                    type: 'fetch',
                    payload: {
                        pageIndex, pageSize
                    }
                })

            }
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