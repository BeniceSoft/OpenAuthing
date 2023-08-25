import { RoleSubjectType } from "@/@types/role";
import { request } from "@/lib/request"

const RoleService = {
    getAll: async (params: { pageIndex: number, pageSize: number }) => {
        const { data } = await request('/api/admin/roles', {
            params
        })
        return data;
    },

    get: async (id: string) => {
        const { data } = await request(`/api/admin/roles/${id}`)
        return data;
    },

    create: async (input: any) => {
        const { data } = await request(`/api/admin/roles`, {
            method: 'POST',
            data: input
        })
        return data;
    },

    update: async (id: string, input: any) => {
        const { data } = await request(`/api/admin/roles/${id}`, {
            method: 'PUT',
            data: input
        })
        return data;
    },

    delete: async (id: string) => {
        const { data } = await request(`/api/admin/roles/${id}`, {
            method: 'DELETE'
        })
        return data;
    },

    toggleEnabled: async (id: string, enabled: boolean) => {
        const { data } = await request(`/api/admin/roles/${id}/toggle-enabled`, {
            method: 'PUT',
            params: {
                enabled
            }
        })
        return data;
    },

    getRoleSubjects: async (id: string) => {
        const { data } = await request(`/api/admin/roles/${id}/subjects`)
        return data;
    },

    saveRoleSubjects: async (id: string, items: Array<{ type: RoleSubjectType, id: string }>) => {
        const { data } = await request(`/api/admin/roles/${id}/subjects`, {
            method: 'PUT',
            data: {
                subjects: items
            }
        })
        return data;
    }
}

export default RoleService