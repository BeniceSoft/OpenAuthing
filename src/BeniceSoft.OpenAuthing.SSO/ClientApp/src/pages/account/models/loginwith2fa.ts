import { useRequest } from "umi"
import AuthService from '@/services/auth.service'
import toast from "react-hot-toast"
import { redirectReturnUrl } from "@/lib/utils"

export default () => {
    const { run: loginWith2Fa } = useRequest(AuthService.loginWith2Fa, {
        manual: true,
        onSuccess(data, params) {
            console.log(data)
            const { returnUrl = "/" } = data

            toast.success('登录成功，正在跳转...')
            redirectReturnUrl(returnUrl)
        },
    })

    return {
        loginWith2Fa
    }
}