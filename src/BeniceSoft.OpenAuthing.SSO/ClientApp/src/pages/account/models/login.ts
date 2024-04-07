import { history, useRequest, formatMessage } from "umi"
import AuthService from "@/services/auth.service"
import toast from "react-hot-toast"
import { redirectReturnUrl } from "@/lib/utils"

export default () => {

    const successMessage = formatMessage({ id: "common.notification.login.success" })

    const { run: loginWithPassword } = useRequest(AuthService.login, {
        manual: true,
        onSuccess(data, params) {
            const { requireTwoFactor, returnUrl, userIndo } = data
            if (!requireTwoFactor) {
                toast.success(successMessage)
                redirectReturnUrl(returnUrl)
                return
            }
            history.push({
                pathname: '/account/loginwith2fa',
                search: "?returnUrl=" + returnUrl
            })
        },
    })

    const { run: loginWith2Fa } = useRequest(AuthService.loginWith2Fa, {
        manual: true,
        onSuccess(data, params) {
            console.log(data)
            const { returnUrl = "/" } = data

            toast.success(successMessage)
            redirectReturnUrl(returnUrl)
        },
    })

    const { run: loginWithRecoveryCode } = useRequest(AuthService.loginWithRecoveryCode, {
        manual: true,
        onSuccess(data, params) {
            const { requireTwoFactor, returnUrl, userIndo } = data
            if (!requireTwoFactor) {
                toast.success(successMessage)
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
        loginWithPassword,
        loginWith2Fa,
        loginWithRecoveryCode
    }
}