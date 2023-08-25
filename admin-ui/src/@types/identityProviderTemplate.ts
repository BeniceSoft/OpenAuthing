export interface idPTemplate {
    name: string
    title: string
    description: string
    logo: string
    fields?: idPTemplateField[]
}

export interface idPTemplateField {
    name: string
    label: string
    placeholder?: string
    helpText?: string
    type: string
    required: boolean
}