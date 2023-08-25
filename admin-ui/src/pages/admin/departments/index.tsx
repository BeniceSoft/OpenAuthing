import PageHeader from "../components/PageHeader"
import Tree from "@/components/Tree"
import { useEffect, useRef, useState } from "react"
import TreeNode from "@/@types/TreeNode"
import { Dispatch, connect } from "umi"
import { DepartmentsModelState } from "@/models/departments"
import InputOrgDialog from "./components/InputOrgDialog"
import classNames from "classnames"
import DepartmentService from "@/services/department.service"
import Message from "@/components/Message"
import { confirm } from '@/components/Modal'
import AddMemberDialog from "./components/AddMemberDialog"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { MoreHorizontal, Plus, Search, XCircle } from "lucide-react"
import { DepartmentMember } from "@/@types/department"
import DepartmentMemberTable from "./components/DepartmentMemberTable"
import { Button } from "@/components/ui/button"
import DepartmentAndUserList from "./components/DepartmentAndUserList"

interface OrgManagementPageProps {
    isLoadingRootDepartment?: boolean
    isLoadingMembers?: boolean
    departmentTree?: TreeNode[]
    departmentMembers?: { totalCount: number, items: DepartmentMember[] }
    dispatch: Dispatch
}

const OrgManagementPage = ({
    isLoadingRootDepartment,
    isLoadingMembers,
    departmentTree,
    departmentMembers,
    dispatch
}: OrgManagementPageProps) => {
    const [searchKey, setSearchKey] = useState('')
    const [selectedNode, setSelectedNode] = useState<TreeNode>()
    const [expandedKeys, setExpendedKeys] = useState(new Set<string>())
    const [messageQueue, setMessageQueue] = useState<Array<{ node: TreeNode, type: 'expand' | 'collapse' }>>([]);
    const [inputOrgDialogState, setInputOrgDialogState] = useState<{ opened: boolean, departmentId?: string }>()
    const [isOrgProcessing, setOrgProcessing] = useState(false)
    const [addMemberDialogOpened, setAddMemberDialogOpened] = useState<boolean>()

    const tableRef = useRef<any>()

    const { totalCount, items: departmentUsers = [] } = departmentMembers ?? {}

    useEffect(() => {
        dispatch({
            type: 'departments/fetchRoot',
            payload: {}
        })

        return () => {
            dispatch({ type: 'departments/clear' })
        }
    }, [])

    useEffect(() => {
        const firstDepartment = departmentTree?.at(0)
        if (selectedNode === undefined && firstDepartment) {
            setSelectedNode(firstDepartment)
        }
    }, [departmentTree])

    // 消费消息队列中的消息
    useEffect(() => {
        if (messageQueue.length > 0) {
            const message = messageQueue.shift();
            if (!message) return

            const newExpandedKeys = new Set(expandedKeys);
            if (message.type === 'expand') {
                setExpendedKeys(newExpandedKeys.add(message.node.key));
            } else if (message.type === 'collapse') {
                newExpandedKeys.delete(message.node.key)
                setExpendedKeys(newExpandedKeys);
            }
        }
    }, [messageQueue]);

    const onSearchKeyChange = (e: any) => {
        setSearchKey(e.target.value.trim())
    }

    // 部门切换时
    const onSelect = (node: TreeNode) => {
        setSelectedNode(node)
    }

    // 处理展开/折叠操作，并将操作封装为消息放入队列中
    const onExpand = (node: TreeNode, expanded: boolean) => {
        setMessageQueue([...messageQueue, { type: expanded ? 'expand' : 'collapse', node }])
    }

    // 加载子部门列表
    const onLoadData = ({ key, children }: TreeNode) => {
        console.log('load data', key)

        return new Promise<void>(resolve => {
            if (children) {
                resolve()
                return
            }

            dispatch({
                type: 'departments/fetchChildren',
                payload: {
                    parentId: key,
                    callback: resolve
                }
            })
        })
    }

    // 打开新建/编辑组织弹窗
    const openOrgDialog = (departmentId?: string) => {
        setInputOrgDialogState({ opened: true, departmentId })
    }

    // 关闭新建/编辑组织弹窗
    const closeOrgDialog = () => {
        setInputOrgDialogState({ opened: false, departmentId: undefined })
    }

    // 处理创建/修改组织
    const handleCreateOrUpdateOrg = async (actionType: string, value: any) => {
        const { id = '' } = value
        setOrgProcessing(true)
        try {
            if (actionType === 'update' && id !== '') {
                const data = await DepartmentService.update(id, { ...value })
                if (data) {
                    Message.success('更新成功')
                }
            } else {
                const data = await DepartmentService.create({ ...value })
                if (data) {
                    Message.success('新建成功')
                }
            }

            closeOrgDialog()
            return true
        }
        catch {
            return false
        }
        finally {
            setOrgProcessing(false)
            dispatch({
                type: 'departments/fetchRoot',
                payload: {}
            })
        }
    }

    const handleDeleteDepartment = (node: TreeNode) => {
        confirm({
            title: `确定删除「${node.title}」吗?`,
            content: '删除后将无法恢复，请谨慎操作！',
            onOK: () => {
                console.log('delete')
            }
        })
    }

    const handleAddMembers = (userIds: string[]) => {
        const pagination = tableRef.current?.currentPagination()
        dispatch({
            type: 'departments/addMembers',
            payload: {
                ...pagination,
                departmentId: selectedNode?.key,
                userIds,
            }
        })

        setAddMemberDialogOpened(false)
    }

    const renderTreeNodeMenu = (node: TreeNode, selected: boolean) => {
        const isOrg = node.parentId === '' || node.parentId === null || typeof node.parentId === 'undefined'
        const typeName = isOrg ? '组织' : '部门'

        return (
            <DropdownMenu>
                <DropdownMenuTrigger asChild={true}>
                    <button className={classNames(
                        "group w-6 h-6 rounded transition-colors flex items-center justify-center",
                        selected ? "text-white hover:bg-gray-200 hover:text-black" : "text-block hover:bg-white/80",
                        'text-gray-700 hidden group-hover:flex aria-expanded:flex'
                    )}>
                        <MoreHorizontal className="w-4 h-4" />
                    </button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="p-2 text-sm text-gray-600">
                    <DropdownMenuGroup>
                        <DropdownMenuItem>
                            <span>添加子部门</span>
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => openOrgDialog(node.key)}>
                            <span>编辑{typeName}</span>
                        </DropdownMenuItem>
                        <DropdownMenuItem onClick={() => handleDeleteDepartment(node)}>
                            <span>删除{typeName}</span>
                        </DropdownMenuItem>
                    </DropdownMenuGroup>
                </DropdownMenuContent>
            </DropdownMenu>
        )
    }

    return (
        <>
            <div className="w-full h-full overflow-y-hidden flex flex-col">
                <PageHeader title="组织管理" description="维护组织内多层级部门，并管理成员的部门隶属关系。" rightRender={() => (
                    <div className="flex gap-x-4">
                        <Button variant="secondary">
                            成员入职
                        </Button>
                        <DropdownMenu>
                            <DropdownMenuTrigger asChild={true}>
                                <Button variant="secondary">
                                    组织成员导入
                                </Button>
                            </DropdownMenuTrigger>
                        </DropdownMenu>
                    </div>
                )} />
                <div className="flex w-full flex-1 mt-6 overflow-y-hidden">
                    <div className="flex flex-col w-64 min-w-[256px] border-r overflow-y-hidden">
                        <div className="flex h-8 w-full mb-2 items-center justify-start pr-5">
                            <div className="flex-1 flex mr-2 w-40 h-full items-center justify-start rounded bg-gray-100 px-2 gap-x-2">
                                <Search className="w-4 h-4 stroke-gray-400" />
                                <input className="flex-1 min-w-0 bg-transparent focus:outline-none text-sm placeholder:text-gray-400"
                                    value={searchKey}
                                    onChange={onSearchKeyChange}
                                    placeholder="搜索成员、部门" />
                                {searchKey.trim() !== '' &&
                                    <span className="w-4 h-4 cursor-pointer">
                                        <XCircle className="w-full h-full fill-gray-400" onClick={() => setSearchKey('')} />
                                    </span>
                                }
                            </div>
                            <DropdownMenu modal={false}>
                                <DropdownMenuTrigger  asChild={true}>
                                    <button className="px-1 gap-x-0 flex items-center justify-center w-full h-full bg-gray-100 text-gray-600 rounded text-sm hover:bg-gray-200 focus-visible:outline-none transition-colors">
                                        <Plus className="w-4 h-4" />
                                        <span>新建</span>
                                    </button>
                                </DropdownMenuTrigger>
                                <DropdownMenuContent align="end" className="p-2 text-sm text-gray-600">
                                    <DropdownMenuGroup>
                                        <DropdownMenuItem>
                                            <span>添加部门</span>
                                        </DropdownMenuItem>
                                        <DropdownMenuItem onClick={() => openOrgDialog()}>
                                            <span>添加组织</span>
                                        </DropdownMenuItem>
                                    </DropdownMenuGroup>
                                </DropdownMenuContent>
                            </DropdownMenu>
                        </div>
                        {searchKey === '' ?
                            <Tree className="flex-1 w-full overflow-y-auto pr-5" isLoading={isLoadingRootDepartment}
                                selectedKey={selectedNode?.key} expandedKeys={expandedKeys}
                                onSelect={onSelect} onLoadData={onLoadData} onExpand={onExpand}
                                treeData={departmentTree}
                                renderMoreMenu={renderTreeNodeMenu} /> :
                            <DepartmentAndUserList loading={true} />
                        }
                    </div>
                    <div className="flex-1 pl-8">
                        <DepartmentMemberTable tableRef={tableRef}
                            isNone={typeof selectedNode === 'undefined' || selectedNode === null}
                            isLoading={isLoadingMembers}
                            departmentId={selectedNode?.key}
                            departmentName={selectedNode?.title}
                            totalCount={totalCount}
                            items={departmentUsers}
                            dispatch={dispatch}
                            onAddMember={() => setAddMemberDialogOpened(true)} />
                    </div>
                </div>
            </div>
            <InputOrgDialog isOpen={inputOrgDialogState?.opened ?? false} onClose={closeOrgDialog}
                currentId={inputOrgDialogState?.departmentId} isProcessing={isOrgProcessing}
                onConfirm={handleCreateOrUpdateOrg} />
            <AddMemberDialog open={addMemberDialogOpened} onOpenChange={setAddMemberDialogOpened}
                department={{ ...selectedNode }}
                onAdd={handleAddMembers} />
        </>
    )
}

export default connect(({ departments, loading }: { loading: any, departments: DepartmentsModelState }) => ({
    isLoadingRootDepartment: loading.effects['departments/fetchRoot'],
    isLoadingMembers: loading.effects['departments/fetchMembers'],
    ...departments
}))(OrgManagementPage)