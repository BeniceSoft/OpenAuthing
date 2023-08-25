import { UserInfo } from "@/@types/user"
import Empty from "@/components/Empty"
import Spin from "@/components/Spin"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import UserService from "@/services/user.service"
import { debounce } from "lodash"
import { UserIcon, X } from "lucide-react"
import React, { ChangeEvent, useCallback, useEffect, useState } from "react"

export interface AddMemberDialogProps {
    department: { key?: string, name?: string }
    open?: boolean
    onOpenChange?: (open: boolean) => void
    onAdd?: (userIds: string[]) => void
}

const AddMemberDialog: React.FC<AddMemberDialogProps> = ({
    open = false,
    onOpenChange,
    department,
    onAdd
}: AddMemberDialogProps) => {
    const [departmentInfo, setDepartmentInfo] = useState<{ id: string, name: string }>()
    const [searchKey, setSearchKey] = useState<string>('')
    const [users, setUsers] = useState<UserInfo[]>([])
    const [selectedUsers, setSelectedUsers] = useState<UserInfo[]>([])
    const [loading, setLoading] = useState<boolean>(false)

    useEffect(() => {
        const { key = '', name = '' } = department || {}
        setDepartmentInfo({ id: key, name })
    }, [department])

    useEffect(() => {
        if (open === false) {
            setSearchKey('')
            setUsers([])
            setSelectedUsers([])
            setDepartmentInfo(undefined)
        } else {
            doSearch()
        }
    }, [open])

    const handleOpenChange = (open: boolean) => {
        onOpenChange && onOpenChange(open)
    }

    const handleSearchKeyChange = (e: ChangeEvent<HTMLInputElement>) => {
        const inputValue = e.target.value;
        setSearchKey(inputValue)

        search(inputValue)
    }

    const doSearch = async (searchKey: string = '') => {
        setLoading(true);
        setUsers([])
        try {
            const { items = [] } = await UserService.getAll({
                searchKey,
                pageIndex: 1,
                pageSize: 20,
                excludeDepartmentId: departmentInfo?.id,
                onlyEnabled: true
            })

            setUsers(items)
        } finally {
            setLoading(false)
        }
    }

    const search = useCallback(debounce(doSearch, 500), [])

    const handleItemSelectedChange = (item: UserInfo, checked: boolean) => {
        if (checked) {
            setSelectedUsers(selectedUsers.concat([item]))
        } else {
            setSelectedUsers(selectedUsers.filter(x => x.id !== item.id))
        }
    }

    const handleConfirm = useCallback(() => {
        onAdd && onAdd(selectedUsers.map(x => x.id))
    }, [selectedUsers])

    return (
        <Dialog defaultOpen={false} open={open} onOpenChange={handleOpenChange}>
            <DialogContent className="sm:max-w-[800px] sm:min-w-[800px]">
                <DialogHeader>
                    <DialogTitle>
                        添加成员
                    </DialogTitle>
                </DialogHeader>
                <div className="flex flex-col h-[450px] gap-y-4">
                    <p className="text-sm text-gray-700">
                        将已选择的成员添加到部门「<span className="font-semibold text-gray-800">{departmentInfo?.name}</span>」中
                        <span className="text-xs text-gray-400">（已过滤已在部门中的成员）</span>
                    </p>
                    <div className="flex-1 w-full overflow-hidden grid grid-cols-2 gap-x-4">
                        <div className="flex flex-col gap-y-4 overflow-hidden">
                            <div className="p-0.5">
                                <Input type="text" sizeVariant="xs" variant="solid"
                                    placeholder="搜索成员" maxLength={50}
                                    value={searchKey}
                                    onChange={handleSearchKeyChange} />
                            </div>
                            <div className="flex-1 overflow-auto">
                                <Spin iconStyle="svg" spinning={loading}>
                                    <Empty isEmpty={!loading && users.length === 0}
                                        description={searchKey === '' ? '没有相关成员' : `没有找到和 "${searchKey}" 相关的成员`}>
                                        <div className="px-0.5">
                                            {users.map((user) => {
                                                const checked = selectedUsers.findIndex(x => x.id === user.id) >= 0
                                                return (
                                                    <div key={user.id}
                                                        className="flex items-center gap-x-2 text-sm rounded p-2 cursor-pointer hover:bg-gray-100 transition-colors"
                                                        onClick={() => handleItemSelectedChange(user, !checked)}>
                                                        <input type="checkbox" checked={checked} readOnly={true}
                                                            className={checked ? 'border-blue-600' : 'border-gray-400'} />
                                                        < Avatar className="w-8 h-8">
                                                            <AvatarImage src={user.avatar} />
                                                            <AvatarFallback>
                                                                <UserIcon className="w-4 h-4 stroke-gray-500" />
                                                            </AvatarFallback>
                                                        </Avatar>
                                                        <div className="">
                                                            <h2>{user.nickname}</h2>
                                                            <p className="text-xs text-gray-500">{user.userName}</p>
                                                        </div>
                                                    </div>
                                                )
                                            })}
                                        </div>
                                    </Empty>
                                </Spin>
                            </div>
                        </div>
                        <div className="flex flex-col gap-y-4 overflow-hidden">
                            {selectedUsers.length > 0 &&
                                <>
                                    <div className="flex h-[38px] items-center justify-between text-sm">
                                        <p>已选：<span className="text-blue-600 pr-1">{selectedUsers.length}</span>名成员</p>
                                        <Button variant="link" onClick={() => setSelectedUsers([])}>清空</Button>
                                    </div>
                                    <div className="flex-1 overflow-auto">
                                        {selectedUsers.map((user) => (
                                            <div key={user.id}
                                                className="flex items-center gap-x-2 text-sm rounded p-2 cursor-pointer hover:bg-gray-100 transition-colors group">
                                                <Avatar className="w-8 h-8">
                                                    <AvatarImage src={user.avatar} />
                                                    <AvatarFallback>
                                                        <UserIcon className="w-4 h-4 stroke-gray-500" />
                                                    </AvatarFallback>
                                                </Avatar>
                                                <div className="flex-1">
                                                    <h2>{user.nickname}</h2>
                                                    <p className="text-xs text-gray-500">{user.userName}</p>
                                                </div>
                                                <Button className="hidden group-hover:block" variant="ghost"
                                                    onClick={() => handleItemSelectedChange(user, false)}>
                                                    <X className="w-4 h-4" />
                                                </Button>
                                            </div>
                                        ))}
                                    </div>
                                </>
                            }
                        </div>
                    </div>
                </div>
                <DialogFooter>
                    <Button variant="secondary" onClick={() => handleOpenChange(false)}>取消</Button>
                    <Button disabled={selectedUsers.length === 0}
                        onClick={handleConfirm}>确定</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog >
    )
}

export default AddMemberDialog