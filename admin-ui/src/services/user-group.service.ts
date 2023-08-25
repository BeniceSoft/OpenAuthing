import { request } from "@/lib/request"

const UserGroupService = {

    getAll: async (params: { search?: string, pageIndex?: number, pageSize?: number }) => {
        const { data } = await request('/api/admin/usergroups', {
            method: 'GET',
            params
        })
        return data
    },

    get: async (id: string) => {
        const { data } = await await request(`/api/admin/usergroups/${id}`)

        return data

    }
}

export default UserGroupService