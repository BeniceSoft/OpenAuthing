import { request } from "@/lib/request"

const DataResourceService = {
    get: async (params: { searchKey?: string, pageIndex?: number, pageSize?: number }) => {
        const { data } = await request('/api/admin/dataresource', {
            method: 'GET',
            params
        })
        return data
    }
}

export default DataResourceService