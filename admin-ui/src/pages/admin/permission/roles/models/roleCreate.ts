import RoleService from "@/services/role.service"
import { useCallback, useState } from "react"
import { toast } from "react-hot-toast"
import { history } from "umi"

export default () => {

    const [creating, setCreating] = useState<boolean>()

    const create = useCallback(async (value: any) => {
        setCreating(true)

        try {
            const result = await RoleService.create(value)
            if (result) {
                toast.success('角色创建成功')
                history.replace('/admin/permission/roles')
            }
        } finally {
            setCreating(false)
        }
    }, [])


    return {
        creating,
        create
    }
}