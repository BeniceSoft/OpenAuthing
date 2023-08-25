
import React from "react"
import { history, useModel } from "umi"
import RoleForm from "./components/RoleForm"

interface CreateRolePageProps {

}

const CreateRolePage: React.FC<CreateRolePageProps> = ({

}: CreateRolePageProps) => {

    const { create, creating } = useModel('admin.permission.roles.roleCreate')

    const handleSubmit = async (value: any) => {
        await create(value)
    }

    return (
        <div className="w-full">
            <div className="mb-2">
                <span onClick={history.back}
                    className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                    </svg>
                    返回
                </span>
            </div>
            <div className="mb-4">
                <h1 className="text-xl font-semibold mb-3">创建角色</h1>
            </div>
            <main className="w-full flex flex-col gap-y-6">
                <div className="mt-4">
                    <RoleForm onSubmit={handleSubmit} isBusy={creating} />
                </div>
            </main>
        </div>
    )
}

export default CreateRolePage