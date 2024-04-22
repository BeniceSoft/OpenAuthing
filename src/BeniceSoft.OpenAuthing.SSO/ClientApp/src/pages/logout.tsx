import { useModel, useRequest, history, Navigate } from "umi";
import AuthService from '@/services/auth.service'
import useReturnUrl from "@/hooks/useReturnUrl";
import { useEffect } from "react";


export default () => {
    const { setInitialState } = useModel('@@initialState')
    const returnUrl = useReturnUrl()

    useEffect(() => {
        setInitialState({
            isAuthenticated: false,
            currentUser: null
        })

        AuthService.logout(returnUrl)
    }, [])

    return (
        <div>
            Logging out, please wait for a moment
        </div>
    )

}