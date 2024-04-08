import { useRequest } from "umi"
import AuthService from '@/services/auth.service'
import { useEffect } from "react"

export default () => {

    const { run: fetchTwoFactorState, data: twoFactorState, loading: twoFactorStateLoading, refresh: refreshTwoFactorState } = useRequest(AuthService.getTwoFactorState, { manual: true })

    return {
        twoFactorState,
        twoFactorStateLoading,
        fetchTwoFactorState,
        refreshTwoFactorState
    }
}