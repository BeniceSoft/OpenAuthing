import PageHeader from "../components/PageHeader"
import { UserInfo } from "@/@types/user"
import { Table, TableRef } from "@/components/Table"
import { Dispatch, connect, history } from "umi"
import { UsersModelState } from "@/models/users"
import React, { useCallback, useEffect, useRef, useState } from "react"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { CheckCircle, CircleSlash, MoreHorizontalIcon, Search, Trash2 } from 'lucide-react'
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Badge } from "@/components/ui/badge"
import CreateUserDialog from "./components/CreateUserDialog"
import { debounce } from "lodash"
import dayjs from "dayjs"

interface UserManagementPageProps {
    dispatch: Dispatch
    totalCount?: number
    users?: UserInfo[]
    isLoading?: boolean
    creating?: boolean
}

const UserManagementPage = ({
    dispatch,
    isLoading,
    totalCount,
    users,
    creating
}: UserManagementPageProps) => {
    const [dialogOpened, setDialogOpened] = useState<boolean>()
    const [searchKey, setSearchKey] = useState<string>('')
    const tableRef = useRef<any>()

    useEffect(() => {
        dispatch({
            type: 'users/fetch',
            payload: {
                pageIndex: 1,
                pageSize: 20
            }
        })

        return () => {
            dispatch({ type: 'users/clear' })
        }
    }, [])

    const handlePageChanged = async ({ pageIndex, pageSize }: { pageSize: number, pageIndex: number }) => {
        dispatch({
            type: 'users/fetch',
            payload: {
                searchKey,
                pageIndex,
                pageSize
            }
        })
    }

    const handleCreate = (input: any) => {
        const { pageIndex, pageSize } = tableRef.current.currentPagination()
        dispatch({
            type: 'users/create',
            payload: {
                input,
                pageIndex,
                pageSize
            }
        })
    }

    const onSearchKeyChange = (event: any) => {
        const inputValue = event.target.value;
        setSearchKey(inputValue);

        search(inputValue)
    }

    const doSearch = (searchKey: string = '') => {
        tableRef.current.resetPagination()
        const { pageIndex, pageSize } = tableRef.current.currentPagination()
        dispatch({
            type: 'users/fetch',
            payload: {
                searchKey,
                pageIndex: 1,
                pageSize
            }
        })
    }

    const search = useCallback(debounce(doSearch, 500), [])

    return (
        <>
            <div className="w-full h-full flex flex-col">
                <PageHeader title="用户管理" description="对组织内所有用户进行统一管理。" rightRender={() => (
                    <Button onClick={() => setDialogOpened(true)}>
                        创建用户
                    </Button>
                )} />
                <div className="flex-1 flex flex-col overflow-hidden gap-y-4 text-sm">
                    <div className="bg-gray-100 p-2 rounded w-1/3 max-w-sm flex gap-x-2 items-center">
                        <Search className="w-5 h-5 text-gray-400" />
                        <input className="flex-1 bg-transparent focus:outline-none placeholder:text-gray-400"
                            placeholder="搜索用户名、昵称、手机号码"
                            maxLength={100}
                            value={searchKey} onChange={onSearchKeyChange} />
                    </div>
                    <Table<UserInfo> ref={tableRef} isLoading={isLoading}
                        totalCount={totalCount ?? 0} items={users}
                        onPageChanged={handlePageChanged}
                        columns={[{
                            title: '用户', dataIndex: 'userName', key: 'userName',
                            render: (value, record) => (
                                <div className="inline-flex items-center gap-x-2 cursor-pointer"
                                    onClick={() => history.push(`/admin/org/users/${record.id}`)}>
                                    <div className="flex gap-x-3 items-center">
                                        <Avatar className="w-8 h-8">
                                            <AvatarImage src={record.avatar}
                                                alt="avatar" />
                                            <AvatarFallback>
                                                <img src="https://files.authing.co/authing-console/default-user-avatar.png" />
                                            </AvatarFallback>
                                        </Avatar>
                                        <div className="flex-1">
                                            <h3 className="text-gray-900 font-medium">{record.nickname}</h3>
                                            <p className="text-xs">{value}</p>
                                        </div>
                                    </div>
                                    {record.isSystemBuiltIn &&
                                        <Badge size="xs" variant="violet">系统内置</Badge>
                                    }
                                </div>
                            )
                        },
                        {
                            title: '状态', dataIndex: 'enabled', key: 'enabled', width: 'w-48', render: (value) => (
                                <Badge variant={value ? 'default' : 'destructive'}>{value ? '启用' : '禁用'}</Badge>
                            )
                        },
                        { title: '手机号码', dataIndex: 'phoneNumber', key: 'phoneNumber', width: 'w-64' },
                        {
                            title: '创建时间', dataIndex: 'creationTime', key: 'creationTime', width: 'w-56', render: (value) => (
                                <span>{dayjs(value).format('YYYY-MM-DD HH:mm:ss')}</span>
                            )
                        },
                        {
                            title: '操作', dataIndex: 'id', key: 'id', width: 'w-20', render: (value, record) => (
                                <DropdownMenu>
                                    <DropdownMenuTrigger asChild>
                                        <Button variant="ghost">
                                            <MoreHorizontalIcon className="w-4 h-4" />
                                        </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end" className="p-2 text-sm">
                                        <DropdownMenuGroup>
                                            <DropdownMenuItem disabled={record.isSystemBuiltIn}
                                                className="flex gap-x-2">
                                                {record.enabled ?
                                                    <>
                                                        <CircleSlash className="w-4 h-4" />
                                                        <span>禁用用户</span>
                                                    </> :
                                                    <>
                                                        <CheckCircle className="w-4 h-4" />
                                                        <span>启用用户</span>
                                                    </>
                                                }
                                            </DropdownMenuItem>
                                            <DropdownMenuItem disabled={record.isSystemBuiltIn}
                                                className="flex gap-x-2 text-destructive">
                                                <Trash2 className="w-4 h-4" />
                                                <span>删除用户</span>
                                            </DropdownMenuItem>
                                        </DropdownMenuGroup>
                                    </DropdownMenuContent>
                                </DropdownMenu>
                            )
                        },
                        ]} />
                </div>
            </div>
            <CreateUserDialog open={dialogOpened} onOpenChange={setDialogOpened}
                creating={creating} onCreate={handleCreate} />
        </>
    )
}

export default connect(({ users, loading }: { users: UsersModelState, loading: any }) => ({
    isLoading: loading.effects['users/fetch'],
    creating: loading.effects['users/create'],
    ...users
}))(UserManagementPage)