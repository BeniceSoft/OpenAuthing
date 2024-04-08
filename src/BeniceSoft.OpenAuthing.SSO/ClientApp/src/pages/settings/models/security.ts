import { useRequest } from "umi"
import AuthService from '@/services/auth.service'

export default () => {

    const { data: twoFactorState, loading: twoFactorStateLoading, refresh: refreshTwoFactorState } = useRequest(AuthService.getTwoFactorState)

    return {
        twoFactorState,
        twoFactorStateLoading,
        refreshTwoFactorState
    }
}