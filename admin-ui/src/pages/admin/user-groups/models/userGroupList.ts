import { UserGroup } from "@/@types/userGroup"
import UserGroupService from "@/services/user-group.service"
import { useCallback, useState } from "react"

export default () => {
    const [loading, setLoading] = useState<boolean>()
    const [pagedResult, setPagedResult] = useState<{ totalCount: number, items?: UserGroup[] }>()

    const fetch = useCallback(async (params?: any) => {
        setLoading(true)
        try {
            const data = await UserGroupService.getAll(params)
            setPagedResult(data)
        } finally {
            setLoading(false)
        }
    }, [])
    const clear = useCallback(() => {
        setLoading(false)
        setPagedResult(undefined)
    }, [])

    return {
        loading,
        pagedResult,
        fetch,
        clear
    }
}