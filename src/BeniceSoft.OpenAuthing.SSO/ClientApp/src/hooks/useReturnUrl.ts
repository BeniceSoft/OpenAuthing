import { getSearchParam } from "@/lib/misc"
import { useMemo } from "react"
import { useSearchParams } from "umi"

const useReturnUrl = () => {
    const [searchParams] = useSearchParams()
    const returnUrl = useMemo(() => getSearchParam(searchParams, 'returnUrl'), [searchParams]) ?? "/"

    return encodeURIComponent(returnUrl)
}

export default useReturnUrl