import { UserProfile } from "@/@types/user"
import AccountService from "@/services/account.service"
import { useCallback, useState } from "react"

export default () => {
    const [loading, setLoading] = useState<boolean>()
    const [profile, setProfile] = useState<UserProfile>()

    const fetch = useCallback(async () => {
        setLoading(true)
        try {
            const profile = await AccountService.getProfile()

            setProfile(profile)
        } finally {
            setLoading(false)
        }
    }, [])

    return {
        loading,
        profile,
        fetch
    }
}