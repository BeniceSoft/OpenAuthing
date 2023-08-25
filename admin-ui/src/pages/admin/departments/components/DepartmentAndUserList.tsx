import { DepartmentModel } from "@/@types/department"
import { UserInfo } from "@/@types/user"
import Empty from "@/components/Empty"
import Spin from "@/components/Spin"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { User, Users } from "lucide-react"
import { history } from "umi"

interface DepartmentAndUserListProps {
    loading?: boolean
    users?: UserInfo[]
    departments?: DepartmentModel[]
}

export default ({
    loading = false,
    users = [],
    departments = []
}: DepartmentAndUserListProps) => {
    return (
        <Spin spinning={loading}>
            <Empty description="没有匹配的数据"
                isEmpty={users.length === 0 && departments.length === 0}>
                <div className="flex-1 py-3 overflow-y-auto pr-5 space-y-4">
                    {users.length > 0 &&
                        <div className="space-y-4">
                            <h2 className="text-xs text-gray-400">成员</h2>
                            <div className="flex flex-col">
                                {users.map(({ id, avatar, nickname, userName }) => (
                                    <div key={id}
                                        className="flex px-1 py-2 rounded items-center text-sm gap-x-2 cursor-pointer hover:bg-gray-100"
                                        onClick={() => history.push(`/admin/org/users/${id}`)}>
                                        <Avatar className="w-8 h-8">
                                            <AvatarImage src={avatar}
                                                alt="avatar" />
                                            <AvatarFallback>
                                                <User className="w-5 h-5 stroke-gray-500" />
                                            </AvatarFallback>
                                        </Avatar>
                                        <div className="flex-1">
                                            <h3 className="text-gray-900 font-medium">{nickname}</h3>
                                            <p className="text-xs">{userName}</p>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    }

                    {departments.length > 0 &&
                        <div className="space-y-4">
                            <h2 className="text-xs text-gray-400">部门</h2>
                            <div className="flex flex-col">
                                {departments.map(({ id, name }) => (
                                    <div key={id}
                                        className="flex px-1 py-2 rounded items-center text-sm gap-x-2 cursor-pointer hover:bg-gray-100"
                                        onClick={() => history.push(`/admin/org/users/${id}`)}>
                                        <Avatar className="w-8 h-8">
                                            <AvatarFallback>
                                                <Users className="w-5 h-5 stroke-gray-500" />
                                            </AvatarFallback>
                                        </Avatar>
                                        <h3 className="text-gray-900 font-medium">{name}</h3>
                                    </div>
                                ))}
                            </div>
                        </div>
                    }
                </div>
            </Empty>
        </Spin>
    )
}