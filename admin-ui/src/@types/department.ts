export interface DepartmentModel {
    id: string
    name: string
    code: string
    description?: string
}

export interface DepartmentMember {
    id: string
    departmentId: string
    userName: string
    nickname: string
    avatar?: string
    enabled: boolean
    phoneNumber: string
    emailAddress: string
    departments?: Array<UserDepartment>
    isLeader: boolean
    isSystemBuiltIn: boolean
}

export interface UserDepartment {
    departmentId: string
    departmentName: string
    isMain: boolean
    isLeader: boolean
}