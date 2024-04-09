import { useModel, useRequest, history } from "umi";
import AuthService from '@/services/auth.service'


export default () => {
    const { setInitialState } = useModel('@@initialState')

    const { loading, error } = useRequest(AuthService.logout, {
        onSuccess: () => {
            setInitialState({
                isAuthenticated: false,
                currentUser: null
            })

            history.replace({
                pathname: '/account/login',
                search: `?returnUrl=/`
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

    return (
        <div>
            You have logged out, you can close the browser
        </div>
    )
}