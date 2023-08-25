import { Reducer, Effect } from 'umi';
import ApplicationService from '@/services/application.service'
import { Client } from '@/@types/openiddict';

export interface ClientsModelState {
    clients?: Client[]
}

export interface ClientsModelType {
    namespace: 'clients';
    state: ClientsModelState;
    effects: {
        fetchAllClients: Effect
    };
    reducers: {
        save: Reducer<ClientsModelState>;
        clear: Reducer<ClientsModelState>
    };
}

const Model: ClientsModelType = {
    namespace: 'clients',
    state: {
    },
    effects: {
        *fetchAllClients({ payload }, { call, put }): any {
            const { searchKey } = payload
            console.log('searchKey', searchKey)
            const data = yield call(ApplicationService.getAll, searchKey)

            yield put({
                type: 'save',
                payload: {
                    clients: data
                }
            });
        }
    },
    reducers: {
        save(state: any, action: any) {
            return {
                ...state,
                ...action.payload,
            };
        },
        clear(state, action) {
            return {
                clients: undefined
            }
        }
    }
}

export default Model;