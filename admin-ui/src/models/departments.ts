import TreeNode from "@/@types/TreeNode"
import { Effect, Reducer } from "umi"
import DepartmentService from "@/services/department.service"
import { toast } from "react-hot-toast"
import { DepartmentMember } from "@/@types/department"

export interface DepartmentsModelState {
    departmentTree?: TreeNode[],
    departmentMembers?: {
        totleCount: number,
        items: DepartmentMember[]
    }
}

interface DepartmentsModelType {
    namespace: 'departments',
    state: DepartmentsModelState,
    effects: {
        fetchRoot: Effect,
        fetchChildren: Effect,
        fetchMembers: Effect,
        addMembers: Effect,
        setLeader: Effect
    },
    reducers: {
        save: Reducer<DepartmentsModelState>,
        clear: Reducer<DepartmentsModelState>
    }
}

const addChildrenToTree = (tree: TreeNode[], parentId: string, children: TreeNode[]) => {
    if (typeof parentId === 'undefined' || parentId === '' || parentId === null) return children
    return tree.map(node => {
        if (node.key === parentId) {
            node.children = children;
        } else if (node.children) {
            node.children = addChildrenToTree(node.children, parentId, children);
        }
        return node;
    });
}

const Model: DepartmentsModelType = {
    namespace: 'departments',
    state: {},
    effects: {
        *fetchRoot({ }, { call, put }): any {
            const data = yield call(DepartmentService.getAll, null)
            yield put({
                type: 'save',
                payload: {
                    departmentTree: data
                }
            })
        },
        *fetchChildren({ payload }, { select, call, put }): any {
            const departmentTree = yield select(({ departments }: any) => departments.departmentTree) ?? []
            const { parentId, callback } = payload
            const data = yield call(DepartmentService.getAll, parentId)
            console.log('data', data)

            const newDepartmentTree = addChildrenToTree(departmentTree as TreeNode[], parentId, data as TreeNode[])

            yield put({
                type: 'save',
                payload: {
                    departmentTree: newDepartmentTree
                }
            })

            callback && callback()
        },
        *fetchMembers({ payload }, { call, put }): any {
            const data = yield call(DepartmentService.getDepartmentMembers, { ...payload })

            yield put({
                type: 'save',
                payload: {
                    departmentMembers: data
                }
            })
        },
        *addMembers({ payload }, { call, put }): any {
            const addedCount = yield call(DepartmentService.addDepartmentMembers, { ...payload })
            toast.success(`已添加 ${addedCount} 个成员`)

            yield put({
                type: 'fetchMembers',
                payload
            })
        },
        *setLeader({ payload }, { call, put }): any {
            const {isLeader} = payload
            const result = yield call(DepartmentService.setLeader, { ...payload })

            if (result) {
                toast.success(`已${isLeader?'设为':'取消'}部门负责人`)

                yield put({
                    type: 'fetchMembers',
                    payload
                })
            }
        }
    },
    reducers: {
        save(state, action) {
            return {
                ...state,
                ...action.payload,
            };
        },
        clear() {
            return {

            }
        }
    }
}

export default Model;