import { DepartmentModel } from "@/@types/department"
import { request } from "@/lib/request"

const DepartmentService = {
    getAll: async (parentId?: string) => {
        const { data } = await request(`/api/admin/departments?parentId=${parentId ?? ''}`)
        return data.map((x: any) => ({ ...x, key: x.id, title: x.name }))
    },

    create: async (input: DepartmentModel) => {
        const { data } = await request('/api/admin/departments', {
            method: 'POST',
            data: input
        })

        return data
    },

    update: async (id: string, input: DepartmentModel) => {
        const { data } = await request(`/api/admin/departments/${id}`, {
            method: 'PUT',
            data: input
        })

        return data
    },



    getDepartmentMembers: async ({
        departmentId, pageIndex = 1, pageSize = 20, onlyDirectUsers
    }: { departmentId: string | null, pageIndex: number | null, pageSize: number | null, onlyDirectUsers: boolean | null }) => {
        const { data } = await request(`/api/admin/departments/${departmentId}/members`, {
            method: 'GET',
            params: {
                pageIndex,
                pageSize,
                onlyDirectUsers
            }
        })

        return data
    },

    addDepartmentMembers: async ({
        departmentId, userIds
    }: { departmentId: string, userIds: string[] }) => {
        const { data } = await request(`/api/admin/departments/${departmentId}/members`, {
            method: 'POST',
            data: {
                userIds
            }
        })
        return data
    },
    setLeader: async ({
        departmentId, userId, isLeader
    }: { departmentId: string, userId: string, isLeader: boolean }) => {
        const { data } = await request(`/api/admin/departments/${departmentId}/members/${userId}/leader`, {
            method: 'PUT',
            params: {
                isLeader
            }
        })
        return data === true
    }
}

export default DepartmentService