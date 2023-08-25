import { MagnifyingGlassIcon } from "@heroicons/react/24/outline"
import PageHeader from "../../components/PageHeader"
import { RoleInfo } from "@/@types/role"
import { Table } from "@/components/Table"
import { Dispatch, Link, connect, history } from "umi"
import React, { useEffect } from "react"
import { RolesModelState } from "@/models/roles"
import {
    Tooltip,
    TooltipArrow,
    TooltipContent,
    TooltipPortal,
    TooltipProvider,
    TooltipTrigger,
} from "@/components/ui/tooltip"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Button } from '@/components/ui/button'
import { CheckCircle, CircleSlash, MoreHorizontalIcon, Trash2 } from "lucide-react"
import { Badge } from "@/components/ui/badge"
import { enabledStatusDescription } from "@/lib/utils"

interface RoleListPageProps {
    dispatch: Dispatch
    loading?: boolean
    totalCount?: number
    items?: RoleInfo[]
}

const RoleListPage: React.FC<RoleListPageProps> = ({
    dispatch,
    loading,
    totalCount,
    items
}: RoleListPageProps) => {
    useEffect(() => {
        dispatch({ type: 'roles/fetch' })

        return () => {
            dispatch({ type: 'roles/clear' })
        }
    }, [])

    const handlePageChanged = ({ pageIndex, pageSize }: any) => new Promise<void>(resolve => {
        dispatch({
            type: 'roles/fetch',
            payload: {
                pageIndex,
                pageSize
            }
        })

        resolve()
    })

    return (
        <div className="w-full h-full flex flex-col">
            <PageHeader title="角色管理"
                description="角色是一个逻辑集合，你可以授权一个角色某些资源与操作权限，当你将角色授予给用户后，该用户将会继承这个角色中的所有权限。"
                rightRender={() => (
                    <Button onClick={() => history.push('/admin/permission/roles/create')}>
                        创建角色
                    </Button>
                )} />

            <div className="flex-1 flex flex-col overflow-hidden gap-y-4 text-sm">
                <div className="bg-gray-100 p-2 rounded w-1/3 max-w-sm flex gap-x-2 items-center">
                    <MagnifyingGlassIcon className="w-5 h-5 text-gray-400" />
                    <input className="flex-1 bg-transparent focus:outline-none placeholder:text-gray-400"
                        placeholder="搜索角色名、编码"
                        maxLength={100} />
                </div>
                <Table<RoleInfo> isLoading={loading}
                    emptyDescription="开始创建角色吧!"
                    totalCount={totalCount} items={items}
                    onPageChanged={handlePageChanged}
                    columns={[
                        {
                            title: '角色显示名', dataIndex: 'displayName', key: 'displayName', render: (value, record) => (
                                <div className="flex gap-x-1 items-center">
                                    <Link to={`/admin/permission/roles/detail/${record.id}`}
                                        className="text-blue-600">
                                        {value}
                                    </Link>
                                    {record.isSystemBuiltIn &&
                                        <Badge variant="violet">系统内置</Badge>
                                    }
                                </div>
                            )
                        },
                        {
                            title: '状态', dataIndex: 'enabled', key: 'enabled', width: 'w-20', render: (value) => (
                                <Badge variant={value ? 'default' : 'destructive'}>
                                    {enabledStatusDescription(value)}
                                </Badge>
                            )
                        },
                        { title: '角色名', dataIndex: 'name', key: 'name', width: 'w-48' },
                        { title: '所属权限空间', dataIndex: 'permissionSpaceName', key: 'permissionSpaceName', width: 'w-48' },
                        {
                            title: '描述', dataIndex: 'description', key: 'description', width: 'w-80', render: (value) => (
                                <TooltipProvider>
                                    <Tooltip>
                                        <TooltipTrigger asChild={true} className="truncate ...">
                                            <div>{value}</div>
                                        </TooltipTrigger>
                                        <TooltipPortal>
                                            <TooltipContent align="start" asChild={true}>
                                                <div className="leading-relaxed max-w-xs text-sm">
                                                    {value}
                                                    <TooltipArrow />
                                                </div>
                                            </TooltipContent>
                                        </TooltipPortal>
                                    </Tooltip>
                                </TooltipProvider>
                            )
                        },
                        {
                            title: '操作', dataIndex: 'id', key: 'id', width: 'w-20', render: (_, { enabled, isSystemBuiltIn }) => (
                                <DropdownMenu>
                                    <DropdownMenuTrigger asChild={true}>
                                        <Button variant="ghost">
                                            <MoreHorizontalIcon className="w-4 h-4" />
                                        </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end">
                                        <DropdownMenuGroup>
                                            <DropdownMenuItem disabled={isSystemBuiltIn} className="flex gap-x-2">
                                                {enabled ?
                                                    <>
                                                        <CircleSlash className="w-4 h-4" />
                                                        <span>禁用角色</span>
                                                    </> :
                                                    <>
                                                        <CheckCircle className="w-4 h-4" />
                                                        <span>启用角色</span>
                                                    </>
                                                }
                                            </DropdownMenuItem>
                                            <DropdownMenuItem disabled={isSystemBuiltIn}
                                                className="flex gap-x-2 text-destructive">
                                                <Trash2 className="w-4 h-4" />
                                                <span>删除角色</span>
                                            </DropdownMenuItem>
                                        </DropdownMenuGroup>
                                    </DropdownMenuContent>
                                </DropdownMenu>
                            )
                        },
                    ]} />
            </div>
        </div>
    )
}

export default connect(({ roles, loading }: { roles: RolesModelState, loading: any }) => ({
    ...roles,
    loading: loading.effects['roles/fetch']
}))(RoleListPage)