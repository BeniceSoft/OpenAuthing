import { RoleInfo, RoleSubject, RoleSubjectType } from "@/@types/role"
import RoleService from "@/services/role.service"
import { useCallback, useState } from "react"
import { toast } from "react-hot-toast"
import { history } from "umi"

export default () => {
    const [loading, setLoading] = useState<boolean>()
    const [updating, setUpdating] = useState<boolean>()
    const [showAddSubjectDialog, setShowAddSubjectDialog] = useState<boolean>()
    const [roleInfo, setRoleInfo] = useState<RoleInfo>()
    const [roleSubjectLoading, setRoleSubjectLoading] = useState<boolean>()
    const [roleSubjects, setRoleSubjects] = useState<RoleSubject[]>();

    const openAddSubjectDialog = useCallback(() => setShowAddSubjectDialog(true), [])
    const closeAddSubjectDialog = useCallback(() => setShowAddSubjectDialog(false), [])
    const fetchRoleInfo = useCallback(async (id: string) => {
        setLoading(true)
        try {
            const data = await RoleService.get(id)
            setRoleInfo(data)

            fetchRoleSubjects(id)
        } finally {
            setLoading(false)
        }
    }, [])
    const fetchRoleSubjects = useCallback(async (id: string) => {
        setRoleSubjectLoading(true)
        try {
            const roleSubjects = await RoleService.getRoleSubjects(id)
            setRoleSubjects(roleSubjects)
        } finally {
            setRoleSubjectLoading(false)
        }
    }, [])
    const update = useCallback(async (id: string, input: any) => {
        setUpdating(true)
        try {
            const result = await RoleService.update(id, input)
            if (result) {
                toast.success("角色更新成功")
                fetchRoleInfo(id)
            }
        } finally {
            setUpdating(false)
        }

    }, [])
    const toggleEnabled = useCallback(async (id: string, enabled: boolean) => {
        const result = await RoleService.toggleEnabled(id, enabled)
        if (result) {
            toast.success(`角色已${enabled ? '启用' : '禁用'}`)
            fetchRoleInfo(id)
        }
    }, [])
    const remove = useCallback(async (id: string) => {
        const result = await RoleService.delete(id)
        if (result) {
            toast.success(`角色删除成功`)
            history.replace('/admin/permission/roles')
        }
    }, [])
    const saveRoleSubjects = useCallback(async (id: string, items: Array<{ type: RoleSubjectType, id: string }>) => {
        const result = await RoleService.saveRoleSubjects(id, items)
        if (result) {
            toast.success(`角色主体已保存`)
            closeAddSubjectDialog()

            fetchRoleSubjects(id)
        }
    }, [])

    const clear = useCallback(() => {
        setLoading(false)
        setUpdating(false)
        setShowAddSubjectDialog(false)
        setRoleInfo(undefined)
    }, [])

    return {
        loading,
        roleInfo,
        fetchRoleInfo,
        roleSubjectLoading,
        roleSubjects,
        updating,
        update,
        showAddSubjectDialog,
        openAddSubjectDialog,
        closeAddSubjectDialog,
        saveRoleSubjects,
        toggleEnabled,
        remove,
        clear
    }
}