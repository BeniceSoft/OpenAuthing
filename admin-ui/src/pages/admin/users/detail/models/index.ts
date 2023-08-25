import { UserInfo } from "@/@types/user"
import UserService from "@/services/user.service"
import { useCallback, useState } from "react"
import { toast } from "react-hot-toast"
import { history } from "umi"

export default () => {
    const [loading, setLoading] = useState<boolean>()
    const [userInfo, setUserInfo] = useState<UserInfo>()

    const fetch = useCallback(async (id: string) => {
        setLoading(true)

        try {
            const userInfo = await UserService.get(id)
            setUserInfo(userInfo)
        } finally {
            setLoading(false)
        }

    }, [])
    const uploadAvatar = useCallback(async (id: string, avatarBlob: Blob) => {
        const result = await UserService.uploadAvatar(id, avatarBlob)
        if (result) {
            toast.success('头像设置成功')

            fetch(id)
        }
    }, [])
    const remove = useCallback(async (id: string) => {
        const result = await UserService.delete(id)

        if (result) {
            toast.success('用户已删除')

            history.replace('/admin/org/users')
        } else {
            toast.error('用户删除失败')
        }
    }, [])


    return {
        loading,
        userInfo,
        fetch,
        uploadAvatar,
        remove
    }
}