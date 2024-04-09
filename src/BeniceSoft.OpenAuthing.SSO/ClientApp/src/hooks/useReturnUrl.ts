import { useMemo } from "react"
import { useSearchParams } from "umi"

const useReturnUrl = () => {
    const [searchParams] = useSearchParams()
    const returnUrl = useMemo(() => searchParams.get('returnUrl'), [searchParams]) ?? ""

    return encodeURIComponent(returnUrl)
}

export default useReturnUrl