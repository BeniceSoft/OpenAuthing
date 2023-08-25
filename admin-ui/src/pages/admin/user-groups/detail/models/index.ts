import { UserGroup } from "@/@types/userGroup"
import UserGroupService from "@/services/user-group.service"
import { useCallback, useState } from "react"

export default () => {
    const [loading, setLoading] = useState<boolean>(false)
    const [userGroup, setUserGroup] = useState<UserGroup>()

    const fetch = useCallback(async (id: string) => {
        setLoading(true)
        try {
            const userGroup = await UserGroupService.get(id)
            setUserGroup(userGroup)
        } finally {
            setLoading(false)
        }
    }, [])

    return ({
        loading,
        userGroup,
        fetch
    })
}