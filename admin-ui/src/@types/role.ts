export interface RoleInfo {
    id: string
    name: string
    displayName: string
    description?: string
    enabled: boolean
    isSystemBuiltIn: boolean
}

export interface RoleSubject {
    id: string
    name: string
    description?: string
    avatar?: string
    subjectType: number
    subjectTypeDescription: string
}

export enum RoleSubjectType {
    User = 0,
    UserGroup = 1
}