import AvatarCorpDialog from "@/components/AvatarCorpDialog"
import { confirm } from "@/components/Modal"
import Spin from "@/components/Spin"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { Tabs, TabsContent, TabsNav, TabsTrigger } from "@/components/ui/tabs"
import { ArrowLeft, CheckCircle, ChevronDown, CircleSlash, Trash2, User } from "lucide-react"
import { lazy, useEffect, useRef, useState } from "react"
import { useParams, history, useModel, Navigate } from "umi"

const UserInfo = lazy(() => import('./components/UserInfo'))
const UserBelong = lazy(() => import('./components/UserBelong'))

export default () => {
    const { id } = useParams()
    if (id === undefined || id === '') {
        return (
            <Navigate to="/admin/org/users" replace={true} />
        )
    }


    const [avatarCorpDialogOpened, setAvatarCorpDialogOpened] = useState<boolean>(false)
    const [avatarSrc, setAvatarSrc] = useState<string>()

    const fileInputRef = useRef<any>()
    const { loading, fetch, userInfo, uploadAvatar, remove } = useModel('admin.users.detail.index')

    useEffect(() => {
        fetch(id)
    }, [id])

    const onAvatarButtonClick = () => {
        fileInputRef.current?.click();
    }

    const onSelectAvatarFile = (e: React.ChangeEvent<HTMLInputElement>) => {
        console.log('onSelectAvatarFile')
        if (e.target.files && e.target.files.length > 0) {
            const reader = new FileReader()
            reader.addEventListener('load', () => {
                console.log('lalalalala')
                setAvatarSrc(reader.result?.toString() || '')
                setAvatarCorpDialogOpened(true)
            },)
            reader.readAsDataURL(e.target.files[0])
        }
    };

    const onAvatarCorpDialogOpenChanged = (open: boolean) => {
        if (fileInputRef.current) {
            fileInputRef.current.value = ''
        }
        setAvatarSrc(undefined);
        setAvatarCorpDialogOpened(open)
    }

    const handleUploadAvatar = async (croppedBlob: Blob | null) => {
        if (croppedBlob) {
            uploadAvatar(id, croppedBlob)
        }
    }

    const handleDelete = () => {
        confirm({
            title: '确认要删除用户吗？',
            content: '删除用户无法恢复，请谨慎操作',
            onOK: () => remove(id)
        })
    }

    return (
        <div className="w-full">
            <Spin spinning={loading ?? false}>
                <div className="mb-2">
                    <span onClick={history.back}
                        className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                        <ArrowLeft className="w-4 h-4" />
                        返回
                    </span>
                </div>
                {userInfo &&
                    <>
                        <div className="flex gap-x-4 items-center mb-4">
                            <div className="flex-1 flex gap-x-4 items-center">
                                <div className="flex-shrink-0 w-14 h-14 rounded-full border overflow-hidden relative cursor-pointer group">
                                    <Avatar className="w-full h-full">
                                        <AvatarImage src={userInfo.avatar} />
                                        <AvatarFallback>
                                            <User className="w-6 h-6 stroke-gray-400" />
                                        </AvatarFallback>
                                    </Avatar>
                                    <span onClick={onAvatarButtonClick}
                                        className="absolute bottom-0 w-full text-center bg-black/50 text-white py-0.5 text-xs hidden group-hover:block">
                                        修改
                                    </span>
                                    <input type="file" ref={fileInputRef}
                                        className="hidden" accept="image/*"
                                        onChangeCapture={onSelectAvatarFile} />
                                </div>
                                <div className="space-y-1">
                                    <div className="flex gap-x-2">
                                        <span className="text-xl font-semibold ">{userInfo.nickname}</span>
                                        <div className="space-x-1">
                                            {userInfo.isSystemBuiltIn &&
                                                <Badge variant="violet">系统内置</Badge>
                                            }
                                            {!userInfo.enabled &&
                                                <Badge variant="destructive">禁用</Badge>
                                            }
                                        </div>
                                    </div>
                                    <p className="text-sm text-gray-400">{userInfo.userName}</p>
                                </div>
                            </div>
                            <div className="flex gap-x-4">
                                <DropdownMenu>
                                    <DropdownMenuTrigger asChild={true}>
                                        <Button variant="secondary" type="button">
                                            更多
                                            <ChevronDown className="w-4 h-4" />
                                        </Button>
                                    </DropdownMenuTrigger>
                                    <DropdownMenuContent align="end" className="p-2 text-sm text-gray-600">
                                        <DropdownMenuGroup>
                                            <DropdownMenuItem disabled={userInfo.isSystemBuiltIn}
                                                className="flex gap-x-2">
                                                {userInfo.enabled ?
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
                                            <DropdownMenuItem disabled={userInfo.isSystemBuiltIn}
                                                className="flex gap-x-2 text-destructive"
                                                onClick={handleDelete}>
                                                <Trash2 className="w-4 h-4" />
                                                <span>删除用户</span>
                                            </DropdownMenuItem>
                                        </DropdownMenuGroup>
                                    </DropdownMenuContent>
                                </DropdownMenu>
                                <Button type="button">
                                    重置密码
                                </Button>
                            </div>
                        </div>
                        <Tabs defaultValue="userinfo">
                            <TabsNav className="border-b">
                                <TabsTrigger value="userinfo">用户信息</TabsTrigger>
                                <TabsTrigger value="userbelong">用户归属</TabsTrigger>
                                <TabsTrigger value="userpermission">权限信息</TabsTrigger>
                            </TabsNav>
                            <div className="py-4">
                                <TabsContent value="userinfo" >
                                    <UserInfo userInfo={userInfo} />
                                </TabsContent>
                                <TabsContent value="userbelong" >
                                    <UserBelong userId={userInfo.id} />
                                </TabsContent>
                                <TabsContent value="userpermission">
                                    权限信息
                                </TabsContent>
                            </div>
                        </Tabs>
                    </>
                }
            </Spin>
            <AvatarCorpDialog open={avatarCorpDialogOpened}
                onOpenChange={onAvatarCorpDialogOpenChanged}
                imageSrc={avatarSrc}
                onConfirm={handleUploadAvatar} />
        </div>
    )
}