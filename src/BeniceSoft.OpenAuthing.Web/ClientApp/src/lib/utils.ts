import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs))
}

export function enabledStatusDescription(enabled: boolean) {
    return enabled ? '启用' : '禁用'
}