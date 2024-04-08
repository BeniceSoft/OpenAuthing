import { history, useRequest, getIntl, useModel } from "umi"
import AuthService from "@/services/auth.service"
import toast from "react-hot-toast"
import { redirectReturnUrl } from "@/lib/utils"

export default () => {
    const { refresh } = useModel('@@initialState', (model) => ({
        ...model.initialState,
        refresh: model.refresh
    }))
    const intl = getIntl()
    const successMessage = intl.formatMessage({ id: "common.notification.login.success" })

    const { run: loginWithPassword } = useRequest(AuthService.login, {
        manual: true,
        async onSuccess(data, params) {
            const { requiresTwoFactor, returnUrl, userInfo } = data
            if (requiresTwoFactor) {
                history.push({
                    pathname: '/account/loginwith2fa',
                    search: "?returnUrl=" + returnUrl
                })
                return
            }
            await refresh()
            toast.success(successMessage)
            redirectReturnUrl(returnUrl)
        },
    })

    const { run: loginWith2Fa } = useRequest(AuthService.loginWith2Fa, {
        manual: true,
        async onSuccess(data, params) {
            console.log(data)
            const { returnUrl = "/" } = data

            await refresh()
            toast.success(successMessage)
            redirectReturnUrl(returnUrl)
        },
    })

    const { run: loginWithRecoveryCode } = useRequest(AuthService.loginWithRecoveryCode, {
        manual: true,
        async onSuccess(data, params) {
            await refresh()
            const { returnUrl, userInfo } = data
            toast.success(successMessage)
            redirectReturnUrl(returnUrl)
        },
    })

    return {
        loginWithPassword,
        loginWith2Fa,
        loginWithRecoveryCode
    }
}