import { useCallback, useState } from "react"

export default () => {
    const [loading, setLoading] = useState<boolean>(false)

    const fetch = useCallback(async (id: string) => {
        setLoading(true)

        try {

        } finally {
            setTimeout(() => {
                setLoading(false)
            }, 2000);
        }
    }, [])

    return ({
        loading,
        fetch
    })
}