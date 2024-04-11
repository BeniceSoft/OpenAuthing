import { useModel, useRequest, history, Navigate } from "umi";
import AuthService from '@/services/auth.service'
import useReturnUrl from "@/hooks/useReturnUrl";


export default () => {
    const { setInitialState } = useModel('@@initialState')
    const returnUrl = useReturnUrl()

    const { loading, error } = useRequest(AuthService.logout, {
        onSuccess: () => {
            setInitialState({
                isAuthenticated: false,
                currentUser: null
            })

            history.replace({
                pathname: decodeURIComponent(returnUrl)
            })
        }
    })

    if (loading) {
        return (
            <div>
                Logging out error
            </div>
        )
    }

    if (error) {
        return (
            <div>
                Logging out, please wait for a moment
            </div>
        )
    }

    return (<Navigate to="/" replace={true} />)
}