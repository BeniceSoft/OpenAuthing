import { RoleSubjectType } from "@/@types/role"
import { UserInfo } from "@/@types/user"
import { UserGroup } from "@/@types/userGroup"
import Spin from "@/components/Spin"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import UserGroupService from "@/services/user-group.service"
import UserService from "@/services/user.service"
import { RadioGroup } from "@headlessui/react"
import classNames from "classnames"
import { Search, User, Users, X } from "lucide-react"
import { useEffect, useState } from "react"

interface SelectionItemProps<TItem extends UserInfo | UserGroup> {
    item: TItem
    showCheckbox?: boolean
    showRemoveButton?: boolean
    checked?: boolean
    onCheck?: () => void
    onRemove?: () => void
}

const UserSelectionItem = ({
    item,
    showCheckbox = false,
    showRemoveButton = false,
    checked = false,
    onCheck,
    onRemove
}: SelectionItemProps<UserInfo>) => {

    const handleClick = () => {
        if (checked) {
            onRemove && onRemove()
            return
        }

        onCheck && onCheck()
    }

    return (
        <div className="flex items-center gap-x-2 hover:bg-gray-50 p-2 cursor-pointer transition-colors rounded select-none"
            onClick={handleClick}>
            {showCheckbox &&
                <input type="checkbox" checked={checked} className={checked ? 'border-blue-600' : 'border-gray-400'}
                    readOnly={true} />
            }
            <Avatar className="w-8 h-8">
                <AvatarImage src={item.avatar} alt="avatar" />
                <AvatarFallback>
                    <User className="w-5 h-5 stroke-gray-500" />
                </AvatarFallback>
            </Avatar>
            <div className="flex-1">
                <h3 className="text-sm text-gray-800">{item.nickname}</h3>
                <p className="text-xs text-gray-400">{item.phoneNumber}</p>
            </div>
            {showRemoveButton &&
                <button className="p-1 rounded transition-colors hover:bg-gray-200"
                    onClick={e => { e.stopPropagation(); onRemove && onRemove() }}>
                    <X className="w-4 h-4" />
                </button>
            }
        </div>
    )
}

const UserGroupSelectionItem = ({
    item,
    showCheckbox = false,
    showRemoveButton = false,
    checked = false,
    onCheck,
    onRemove
}: SelectionItemProps<UserGroup>) => {

    const handleClick = () => {
        if (checked) {
            onRemove && onRemove()
            return
        }

        onCheck && onCheck()
    }

    return (
        <div className="flex items-center gap-x-2 hover:bg-gray-50 p-2 cursor-pointer transition-colors rounded select-none"
            onClick={handleClick}>
            {showCheckbox &&
                <input type="checkbox" checked={checked} className={checked ? 'border-blue-600' : 'border-gray-400'}
                    readOnly={true} />
            }
            <Avatar className="w-8 h-8">
                <AvatarFallback>
                    <Users className="w-5 h-5 stroke-gray-500" />
                </AvatarFallback>
            </Avatar>
            <div className="flex-1">
                <h3 className="text-sm text-gray-800">{item.name}</h3>
            </div>
            {showRemoveButton &&
                <button className="p-1 rounded transition-colors hover:bg-gray-200"
                    onClick={e => { e.stopPropagation(); onRemove && onRemove() }}>
                    <X className="w-4 h-4" />
                </button>
            }
        </div>
    )
}

type SelectionActionType = 'checked' | 'removed'

interface SelectionProps<TItem extends UserInfo | UserGroup> {
    checkedItems?: TItem[]
    className?: string
    onChange?: (action: SelectionActionType, changedItem: TItem) => void
}

const UserSelection = ({
    className,
    checkedItems = [],
    onChange
}: SelectionProps<UserInfo>) => {
    const [loading, setLoading] = useState<boolean>()
    const [users, setUsers] = useState<UserInfo[]>()

    const fetch = async () => {
        setLoading(true)
        try {
            const { items } = await UserService.getAll({ pageIndex: 1, pageSize: 50 })
            setUsers(items)
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { fetch() }, [])

    const handleItemCheck = (user: UserInfo) => {

        triggerChanged('checked', user)
    }

    const handleItemRemove = (user: UserInfo) => {

        triggerChanged('removed', user)
    }

    const triggerChanged = (action: SelectionActionType, changedItem: UserInfo) => {
        onChange && onChange(action, changedItem)
    }

    return (
        <div className={classNames("flex flex-col gap-y-4 overflow-hidden", className)}>
            <div className="flex items-center p-2 gap-x-1 rounded bg-gray-50">
                <Search className="w-4 h-4 text-gray-400" />
                <input className="flex-1 text-sm bg-transparent focus:outline-none placeholder:text-gray-400"
                    placeholder="关键字查询"
                    maxLength={100} />
            </div>
            <div className="flex-1 overflow-auto flex flex-col gap-y-2">
                <Spin spinning={loading ?? false}>
                    {users && users.map((user) => (
                        <UserSelectionItem key={user.id}
                            item={user}
                            showCheckbox={true}
                            onCheck={() => handleItemCheck(user)}
                            onRemove={() => handleItemRemove(user)}
                            checked={checkedItems.findIndex(x => x.id === user.id) >= 0} />
                    ))}
                </Spin>
            </div>
        </div>
    )
}

const UserGroupSelection = ({
    className,
    checkedItems = [],
    onChange
}: SelectionProps<UserGroup>) => {
    const [loading, setLoading] = useState<boolean>()
    const [userGroups, setUserGroups] = useState<UserGroup[]>()

    const fetch = async () => {
        setLoading(true)
        try {
            const { items } = await UserGroupService.getAll({ pageIndex: 1, pageSize: 50 })
            setUserGroups(items)
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { fetch() }, [])

    const handleItemCheck = (user: UserGroup) => {

        triggerChanged('checked', user)
    }

    const handleItemRemove = (user: UserGroup) => {

        triggerChanged('removed', user)
    }

    const triggerChanged = (action: SelectionActionType, changedItem: UserGroup) => {
        onChange && onChange(action, changedItem)
    }

    return (
        <div className={classNames("h-full flex flex-col gap-y-4 overflow-hidden", className)}>
            <div className="flex items-center p-2 gap-x-1 rounded bg-gray-50">
                <Search className="w-4 h-4 text-gray-400" />
                <input className="flex-1 text-sm bg-transparent focus:outline-none placeholder:text-gray-400"
                    placeholder="关键字查询"
                    maxLength={100} />
            </div>
            <div className="flex-1 overflow-auto flex flex-col gap-y-2">
                <Spin spinning={loading ?? false}>
                    {userGroups && userGroups.map((userGroup) => (
                        <UserGroupSelectionItem key={userGroup.id}
                            item={userGroup}
                            showCheckbox={true}
                            onCheck={() => handleItemCheck(userGroup)}
                            onRemove={() => handleItemRemove(userGroup)}
                            checked={checkedItems.findIndex(x => x.id === userGroup.id) >= 0} />
                    ))}
                </Spin>
            </div>
        </div>
    )
}

type SelectedItemType = {
    type: RoleSubjectType,
    item: UserInfo | UserGroup
}

export interface AddRoleSubjectDialogProps {
    isOpen?: boolean
    onClose?: () => void
    onSave?: (items: SelectedItemType[]) => Promise<void>
}

const AddRoleSubjectDialog = ({
    isOpen = false,
    onClose,
    onSave
}: AddRoleSubjectDialogProps) => {
    const [items, setItems] = useState<SelectedItemType[]>([]);
    const [currentSubjectType, setCurrentSubjectType] = useState<RoleSubjectType>(RoleSubjectType.User)

    const handleClose = () => {
        setItems([])
        onClose && onClose()
    }

    const handleSave = async () => {
        onSave && await onSave(items);
        setItems([])
    }

    const handleSelectionChanged = (type: RoleSubjectType, action: SelectionActionType, changedItem: UserInfo | UserGroup) => {
        if (action === 'checked') {
            setItems(items.concat({ type, item: changedItem }))
        } else {
            setItems(items.filter(x => !(x.type === type && x.item.id === changedItem.id)))
        }
    }

    return (
        <Dialog open={isOpen} onOpenChange={handleClose}>
            <DialogContent className="h-[548px] flex flex-col sm:max-w-4xl sm:min-w-4xl">
                <DialogHeader>
                    <DialogTitle>
                        添加角色主体
                    </DialogTitle>
                </DialogHeader>
                <div className="flex-1 flex flex-col rounded px-8 py-4 -mx-6 -mt-4 -mb-6 bg-gray-50 overflow-hidden">
                    <div className="flex-1 grid grid-cols-2 gap-x-4 bg-gray-50 overflow-hidden">
                        <div className="bg-white flex flex-col gap-y-2 overflow-hidden p-4">
                            <div>
                                <RadioGroup as="div"
                                    className="inline-flex text-sm rounded overflow-hidden"
                                    value={currentSubjectType} onChange={setCurrentSubjectType}>
                                    <RadioGroup.Option value={RoleSubjectType.User}>
                                        {({ checked }) => (
                                            <button className={classNames("py-1.5 px-8 cursor-pointer", checked ? "bg-blue-600 text-white" : 'bg-gray-100')}>
                                                用户
                                            </button>
                                        )}
                                    </RadioGroup.Option>
                                    <RadioGroup.Option value={RoleSubjectType.UserGroup}>
                                        {({ checked }) => (
                                            <button className={classNames("py-1.5 px-8 cursor-pointer", checked ? "bg-blue-600 text-white" : 'bg-gray-100')}>
                                                用户组
                                            </button>
                                        )}
                                    </RadioGroup.Option>
                                </RadioGroup>
                            </div>
                            <UserGroupSelection className={currentSubjectType !== RoleSubjectType.UserGroup ? "hidden" : undefined}
                                checkedItems={items.filter(x => x.type === RoleSubjectType.UserGroup).map(x => x.item as UserGroup)}
                                onChange={(action, changedItem) => handleSelectionChanged(RoleSubjectType.UserGroup, action, changedItem)} />
                            <UserSelection className={currentSubjectType !== RoleSubjectType.User ? "hidden" : undefined}
                                checkedItems={items.filter(x => x.type === RoleSubjectType.User).map(x => x.item as UserInfo)}
                                onChange={(action, changedItem) => handleSelectionChanged(RoleSubjectType.User, action, changedItem)} />
                        </div>
                        <div className="bg-white overflow-auto p-4">
                            {items.map(({ type, item }) => (
                                type === RoleSubjectType.User ?
                                    <UserSelectionItem key={item.id} item={item as UserInfo} showRemoveButton={true}
                                        onRemove={() => handleSelectionChanged(RoleSubjectType.User, 'removed', item)} /> :
                                    <UserGroupSelectionItem key={item.id} item={item as UserGroup} showRemoveButton={true}
                                        onRemove={() => handleSelectionChanged(RoleSubjectType.UserGroup, 'removed', item)} />
                            ))}
                        </div>
                    </div>
                    <div className="flex justify-end gap-x-2 pt-4">
                        <button onClick={handleClose}
                            className="px-4 py-1.5 text-sm rounded bg-gray-200 hover:bg-gray-300 transition-colors">
                            取消
                        </button>
                        <button
                            onClick={handleSave}
                            className="px-4 py-1.5 text-sm rounded text-white bg-blue-600 hover:bg-blue-700 transition-colors">
                            保存
                        </button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}

export default AddRoleSubjectDialog