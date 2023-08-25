import { request } from "@/lib/request"

const UserService = {

    getAll: async (params: { searchKey?: string, pageIndex?: number, pageSize?: number, excludeDepartmentId?: string, onlyEnabled?: boolean }) => {
        const { data } = await request('/api/admin/users', {
            method: 'GET',
            params
        })
        return data
    },

    get: async (id: string) => {
        const { data } = await request(`/api/admin/users/${id}`, {
            method: 'GET'
        })
        return data
    },

    create: async (input: any) => {
        const { data } = await request('/api/admin/users', {
            method: 'POST',
            data: {
                ...input
            }
        })

        return data
    },

    delete: async (id: string) => {
        const { data } = await request(`/api/admin/users/${id}`, {
            method: 'DELETE'
        })
        return data === true
    },

    uploadAvatar: async (id: string, avatarBlob: Blob) => {
        const formData = new FormData()
        formData.append('file', avatarBlob)
        const { data } = await request(`/api/admin/users/${id}/avatar`, {
            method: 'put',
            data: formData,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })

        return data === true
    },

    getUserDepartments: async (id: string) => {
        const { data } = await request(`/api/admin/users/${id}/departments`, {
            method: 'GET'
        })
        return data
    }
}

export default UserService