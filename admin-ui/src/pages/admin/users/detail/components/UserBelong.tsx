import { UserDepartment } from "@/@types/department"
import Empty from "@/components/Empty"
import Spin from "@/components/Spin"
import { Table } from "@/components/Table"
import { Badge } from "@/components/ui/badge"
import UserService from "@/services/user.service"
import classNames from "classnames"
import { Star } from "lucide-react"
import { useCallback, useEffect, useState } from "react"

interface UserBelongProps {
    userId: string
}

export default ({
    userId
}: UserBelongProps) => {

    const [departmentLoading, setDeaprtmentLoading] = useState<boolean>()
    const [departments, setDepartments] = useState<UserDepartment[]>()

    useEffect(() => {
        fetchDepartments()
    }, [userId])

    const fetchDepartments = useCallback(async () => {
        setDeaprtmentLoading(true)

        try {
            const data = await UserService.getUserDepartments(userId)
            setDepartments(data)
        } finally {
            setDeaprtmentLoading(false)
        }
    }, [userId])

    return (
        <div className="space-y-8">
            <div className="space-y-4">
                <h2 className="text-lg font-medium">所属部门</h2>
                <Spin spinning={departmentLoading ?? false}>
                    <Empty isEmpty={(departments ?? []).length === 0}>
                        <div className="">
                            {departments?.map(({ departmentName, isMain, isLeader }) => (
                                <div className="h-10 flex items-center gap-x-2">
                                    <div className="text-sm">
                                        {departmentName}
                                        {isMain &&
                                            <span className="text-xs text-gray-400">（主部门）</span>
                                        }
                                    </div>
                                    {isLeader &&
                                        <Badge>部门负责人</Badge>
                                    }
                                </div>
                            ))}
                        </div>
                    </Empty>
                </Spin>
            </div>
            <div>
                <h2 className="text-lg font-medium">拥有角色</h2>
                <Spin spinning={false}>
                    <Empty isEmpty={true}>
                        <div>
                            
                        </div>
                    </Empty>
                </Spin>
            </div>
        </div>
    )
}