import { history, useRequest } from "umi"
import AuthService from "@/services/auth.service"
import toast from "react-hot-toast"
import { redirectReturnUrl } from "@/lib/utils"

export default () => {

    const { run: loginWithPassword, loading: isLoggingIn } = useRequest(AuthService.login, {
        manual: true,
        onSuccess(data, params) {
            const { requireTwoFactor, returnUrl, userIndo } = data
            if (!requireTwoFactor) {
                toast.success('登录成功，正在跳转...')
                redirectReturnUrl(returnUrl)
                return
            }
            history.push({
                pathname: '/account/loginwith2fa',
                search: "?returnUrl=" + returnUrl
            })
        },
    })

    return {
        isLoggingIn,
        loginWithPassword
    }
}