import AccountService from "@/services/account.service"
import { useRequest } from "umi"

export default () => {
    const {loading, run: fetch, data: profile } = useRequest(AccountService.getProfile)

    return {
        loading,
        profile,
        fetch
    }
}