import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs))
}

export function enabledStatusDescription(enabled: boolean) {
    return enabled ? '启用' : '禁用'
}

export function redirectReturnUrl(returnUrl: string) {
    const redirectUrl = decodeURIComponent(returnUrl)
    console.log("redirecting: ", redirectUrl)
    setTimeout(() => {
        window.location.href = redirectUrl
    }, 500);
}