import SearchInput from "@/components/SearchInput"
import Spin from "@/components/Spin"
import { Table, TableRef } from "@/components/Table"
import { Badge } from "@/components/ui/badge"
import { forwardRef, useEffect, useRef } from "react"
import { Link, useModel } from "umi"
import { DataTypes } from ".."
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { MoreHorizontalIcon, Trash2 } from "lucide-react"
import { confirm } from '@/components/Modal'

export default forwardRef<any>(({ }, _) => {
    const { loading, data, fetch, remove } = useModel('admin.permission.data.resources.management')
    const tableRef = useRef<TableRef>(null)

    useEffect(() => {
        fetch()
    }, [])

    const onDelete = (id: string, name: string) => {
        confirm({
            title: `确定删除「${name}」吗？`,
            content: '此操作将在关联的策略中移除该资源。删除后将无法恢复，请谨慎操作！',
            onOK: () => {
                remove(id)
                tableRef.current?.resetPagination()
            }
        })
    }

    return (
        <Spin spinning={loading}>
            <div className="flex-1 flex flex-col gap-y-4 overflow-hidden">
                <div className="w-1/3 max-w-sm">
                    <SearchInput placeholder="搜索资源名称、资源标识" />
                </div>
                <div className="flex-1 overflow-auto">
                    <Table<any> ref={tableRef} {...data}
                        columns={[{
                            title: '资源名称', dataIndex: 'name', key: 'name', render: (value, record) => (
                                <Link className="text-primary" to={`/admin/permission/data/resources/detail/${record.id}`}>{value}</Link>
                            )
                        }, {
                            title: '资源标识', dataIndex: 'code', key: 'code'
                        }, {
                            title: '资源类型', dataIndex: 'type', key: 'type', render: (value) => (
                                <Badge>{DataTypes[value]?.title}</Badge>
                            )
                        }, {
                            title: '资源描述', dataIndex: 'description', key: 'description'
                        }, {
                            title: '最后编辑时间', dataIndex: 'latestModificationTime', key: 'latestModificationTime'
                        }, {
                            title: '操作', dataIndex: 'id', key: 'id', width: 'w-20', render: (value, record) => (
                                <DropdownMenu modal={false}>
                                    <DropdownMenuTrigger asChild>
                                        <Button variant="ghost">
                                            <MoreHorizontalIcon className="w-4 h-4" />
                                        </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end" className="p-2 text-sm">
                                        <DropdownMenuGroup>
                                            <DropdownMenuItem
                                                onClick={() => onDelete(value, record.name)}
                                                className="flex gap-x-2 text-destructive">
                                                <Trash2 className="w-4 h-4" />
                                                <span>删除</span>
                                            </DropdownMenuItem>
                                        </DropdownMenuGroup>
                                    </DropdownMenuContent>
                                </DropdownMenu>
                            )
                        }]} />
                </div>
            </div>
        </Spin>
    )
})