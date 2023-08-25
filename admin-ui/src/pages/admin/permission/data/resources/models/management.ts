import DataResourceService from "@/services/data-resource.service"
import { useState } from "react"

export default () => {
    const [loading, setLoading] = useState<boolean>(false)
    const [data, setData] = useState<{ totalCount?: number, items?: [] }>({})

    const fetch = async (pageIndex?: number, pageSize?: number, searchKey?: string) => {
        setLoading(true)

        try {
            const data = await DataResourceService.get({ pageIndex, pageSize, searchKey })
            setData(data)
        }
        finally {
            setLoading(false)
        }
    }

    const remove = (id: string) => {
        // TODO: remove data resource

        fetch()
    }

    return {
        loading,
        data,
        fetch,
        remove
    }
}