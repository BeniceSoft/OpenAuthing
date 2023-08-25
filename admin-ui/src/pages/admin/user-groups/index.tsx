import { Table } from "@/components/Table"
import PageHeader from "../components/PageHeader"
import { UserGroup } from "@/@types/userGroup"
import { MagnifyingGlassIcon } from "@heroicons/react/24/outline"
import { Link, history, useModel } from "umi"
import { useEffect } from "react"
import dayjs from "dayjs"
import { Badge } from "@/components/ui/badge"
import { enabledStatusDescription } from "@/lib/utils"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { CircleSlash, MoreHorizontal, Trash2 } from "lucide-react"

const UserGroupManagementPage = () => {
    const {
        loading,
        pagedResult,
        fetch,
        clear
    } = useModel('admin.user-groups.userGroupList')

    useEffect(() => {
        fetch()

        return clear
    }, [])

    return (
        <div className="w-full h-full flex flex-col">
            <PageHeader title="用户组管理" description="对相同属性的用户进行分组后统一管理。" rightRender={() => (
                <Button onClick={() => history.push('/admin/org/user-groups/create')}>
                    创建用户组
                </Button>
            )} />
            <div className="flex-1 flex flex-col overflow-hidden gap-y-4 text-sm">
                <div className="bg-gray-100 p-2 rounded w-1/3 max-w-sm flex gap-x-2 items-center">
                    <MagnifyingGlassIcon className="w-5 h-5 text-gray-400" />
                    <input className="flex-1 bg-transparent focus:outline-none placeholder:text-gray-400"
                        placeholder="搜索名称、标识"
                        maxLength={100} />
                </div>
                <Table<UserGroup> isLoading={loading}
                    emptyDescription="尝试新建用户组吧!"
                    {...pagedResult}
                    columns={[
                        {
                            title: '显示名', dataIndex: 'displayName', key: 'displayName', render: (value, { id }) => (
                                <div className="cursor-pointer text-gray-900"
                                    onClick={() => history.push(`/admin/org/user-groups/detail/${id}`)}>
                                    {value}
                                </div>
                            )
                        },
                        { title: '名称', dataIndex: 'name', key: 'name' },
                        {
                            title: '状态', dataIndex: 'enabled', key: 'enabled', width: 'w-32', render: (value) => (
                                <Badge variant={value ? 'default' : 'destructive'}>{enabledStatusDescription(value)}</Badge>
                            )
                        },
                        {
                            title: '创建时间', dataIndex: 'creationTime', key: 'creationTime', width: 'w-64', render: (value) => (
                                <span>{dayjs(value).format('YYYY-MM-DD HH:mm')}</span>
                            )
                        },
                        {
                            title: '操作', dataIndex: 'id', key: 'id', width: 'w-20', render: (value, record) => (
                                <DropdownMenu>
                                    <DropdownMenuTrigger asChild>
                                        <Button variant="ghost">
                                            <MoreHorizontal className="w-4 h-4" />
                                        </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end" className="p-2 text-sm">
                                        <DropdownMenuGroup>
                                            <DropdownMenuItem className="flex gap-x-2">
                                                <CircleSlash className="w-4 h-4" />
                                                <span>{enabledStatusDescription(!record.enabled)}用户组</span>
                                            </DropdownMenuItem>
                                            <DropdownMenuItem className="flex gap-x-2 text-destructive">
                                                <Trash2 className="w-4 h-4" />
                                                <span>删除用户组</span>
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

export default UserGroupManagementPage