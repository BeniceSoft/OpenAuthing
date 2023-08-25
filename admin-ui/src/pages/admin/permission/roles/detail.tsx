import { RoleSubject } from "@/@types/role";
import Spin from "@/components/Spin";
import { Table } from "@/components/Table";
import { Fragment, useEffect } from "react";
import { history, useModel, useParams } from "umi";
import RoleForm from "./components/RoleForm";
import { CheckCircleIcon, NoSymbolIcon, PlusIcon, TrashIcon } from "@heroicons/react/24/outline";
import AddRoleSubjectDialog from "./components/AddRoleSubjectDialog";
import { Menu, Transition } from "@headlessui/react";
import ConfirmModal from "@/components/Modal/ConfirmModal";
import dayjs from "dayjs";
import { Badge } from "@/components/ui/badge";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { ChevronDown, MoreHorizontalIcon, User, UserMinusIcon, Users } from "lucide-react";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";

export default () => {
    const { id } = useParams()
    const {
        loading, roleInfo, fetchRoleInfo,
        roleSubjectLoading, roleSubjects,
        update, updating,
        showAddSubjectDialog, openAddSubjectDialog, closeAddSubjectDialog, saveRoleSubjects,
        toggleEnabled, remove, clear
    } = useModel('admin.permission.roles.roleDetail')

    useEffect(() => {
        fetchRoleInfo(id!)

        return clear
    }, [id])

    const handleSubmit = async (value: any) => {
        await update(id!, value)
    }

    const handleToggleEnabled = (enabled: boolean) => {
        const title = `确定${enabled ? '启用' : '禁用'}「${roleInfo!.displayName}」吗？`
        const content = enabled ?
            "启用后，角色主体、关联授权也将立即生效。" :
            <div className="leading-relaxed">
                <p>禁用后，角色主体将不具有此角色的关联资源权限；</p>
                <p>角色禁用期间，仍可查看未移除的角色主体和授权；</p>
                <p>若需恢复，可启用角色；</p>
            </div>

        ConfirmModal.confirm({
            title,
            content,
            onOK: () => {
                return toggleEnabled(id!, enabled)
            }
        })
    }

    const handleDelete = () => {
        ConfirmModal.confirm({
            title: `确定删除角色「${roleInfo!.displayName}」吗`,
            content: '删除后将无法恢复，请谨慎操作！',
            onOK: () => {
                return remove(id!)
            }
        })
    }

    const handleSaveRoleSubjects = async (items: any) => {
        await saveRoleSubjects(id!, items.map((x: any) => ({ type: x.type, id: x.item.id })))
    }

    return (
        <>
            <div className="w-full">
                <Spin spinning={loading ?? false}>
                    <div className="mb-2">
                        <span onClick={history.back}
                            className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                            </svg>
                            返回
                        </span>
                    </div>
                    {roleInfo &&
                        <>
                            <div className="flex gap-x-4 items-center mb-4">
                                <div className="flex-1 flex gap-x-2 items-center">
                                    <h1 className="text-xl font-semibold">
                                        {roleInfo.displayName}
                                    </h1>
                                    {roleInfo.isSystemBuiltIn &&
                                        <Badge variant="violet">系统内置</Badge>
                                    }
                                    {!roleInfo.enabled &&
                                        <Badge variant="destructive">已禁用</Badge>
                                    }
                                </div>
                                <div className="flex gap-x-4">
                                    <DropdownMenu>
                                        <DropdownMenuTrigger asChild={true}>
                                            <Button variant="secondary">
                                                更多操作
                                                <ChevronDown className="w-4 h-4" />
                                            </Button>
                                        </DropdownMenuTrigger>
                                        <DropdownMenuContent align="end">
                                            <DropdownMenuItem className="flex gap-x-2 px-2"
                                                disabled={roleInfo.isSystemBuiltIn}
                                                onClick={() => handleToggleEnabled(!roleInfo.enabled)}>
                                                {roleInfo.enabled ?
                                                    <>
                                                        <NoSymbolIcon className="w-4 h-4" />
                                                        <span>禁用角色</span>
                                                    </> :
                                                    <>
                                                        <CheckCircleIcon className="w-4 h-4" />
                                                        <span>启用角色</span>
                                                    </>
                                                }
                                            </DropdownMenuItem>
                                            <DropdownMenuItem className="flex gap-x-2 text-destructive"
                                                disabled={roleInfo.isSystemBuiltIn}
                                                onClick={handleDelete}>
                                                <TrashIcon className="w-4 h-4" />
                                                删除角色
                                            </DropdownMenuItem>
                                        </DropdownMenuContent>
                                    </DropdownMenu>
                                </div>
                            </div>
                            <main className="w-full flex flex-col gap-y-8">
                                <div className="mt-4">
                                    <RoleForm disabled={roleInfo.isSystemBuiltIn || !roleInfo.enabled} initValue={roleInfo} onSubmit={handleSubmit} isBusy={updating} />
                                </div>
                                <div className="flex flex-col gap-y-2">
                                    <div className="flex items-center">
                                        <h2 className="flex-1 font-medium">角色主体</h2>
                                        {roleInfo.enabled &&
                                            <button type="button"
                                                className="flex gap-x-1 text-blue-600 items-center text-sm transition-colors hover:bg-gray-200 px-2 py-1.5 rounded"
                                                onClick={openAddSubjectDialog}>
                                                <PlusIcon className="w-4 h-4" />
                                                添加主体
                                            </button>
                                        }
                                    </div>
                                    <Table<RoleSubject> isLoading={roleSubjectLoading}
                                        items={roleSubjects}
                                        showPagination={false} columns={[{
                                            key: 'name',
                                            dataIndex: 'name',
                                            title: '主体名称',
                                            render: (value, { avatar, description, subjectType }) => (
                                                <div className="inline-flex gap-x-3 items-center">
                                                    <Avatar className="w-8 h-8">
                                                        <AvatarImage src={avatar}
                                                            alt="avatar" />
                                                        <AvatarFallback>
                                                            {subjectType === 1 ?
                                                                <Users className="w-5 h-5 stroke-gray-500" /> :
                                                                <User className="w-5 h-5 stroke-gray-500" />
                                                            }
                                                        </AvatarFallback>
                                                    </Avatar>
                                                    <div className="flex-1">
                                                        <h3 className="text-gray-900 font-medium">{value}</h3>
                                                        <p className="text-xs">{description}</p>
                                                    </div>
                                                </div>
                                            )
                                        }, {
                                            key: 'subjectTypeDescription',
                                            dataIndex: 'subjectTypeDescription',
                                            title: '主体类型',
                                            width: 'w-48'
                                        }, {
                                            key: 'creationTime',
                                            dataIndex: 'creationTime',
                                            title: '添加时间',
                                            width: 'w-64',
                                            render: (value) => (
                                                <span>{dayjs(value).format('YYYY-MM-DD HH:mm:ss')}</span>
                                            )
                                        }, {
                                            key: 'id',
                                            dataIndex: 'id',
                                            title: '操作',
                                            width: 'w-20',
                                            render: () => (
                                                <DropdownMenu>
                                                    <DropdownMenuTrigger asChild={true}>
                                                        <Button variant="ghost">
                                                            <MoreHorizontalIcon className="w-4 h-4" />
                                                        </Button>
                                                    </DropdownMenuTrigger>
                                                    <DropdownMenuContent align="end" className="w-24">
                                                        <DropdownMenuGroup>
                                                            <DropdownMenuItem className="flex items-center text-sm gap-x-2 text-gray-600">
                                                                <UserMinusIcon className="w-4 h-4" />
                                                                <span>移除</span>
                                                            </DropdownMenuItem>
                                                        </DropdownMenuGroup>
                                                    </DropdownMenuContent>
                                                </DropdownMenu>
                                            )
                                        }]} />
                                </div>
                            </main>
                        </>
                    }
                </Spin>
            </div>
            <AddRoleSubjectDialog isOpen={showAddSubjectDialog} onClose={closeAddSubjectDialog} onSave={handleSaveRoleSubjects} />
        </>
    )
}