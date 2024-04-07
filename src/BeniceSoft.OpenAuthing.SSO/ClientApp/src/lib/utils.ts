import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
    return twMerge(clsx(inputs))
}

export function enabledStatusDescription(enabled: boolean) {
    return enabled ? '启用' : '禁用'
}

export function redirectReturnUrl(returnUrl: string) {
    console.log("return url is: ", returnUrl)
    if (!returnUrl.startsWith("/connect")) {
        returnUrl = returnUrl.ensureStartsWith("#")
    }
    window.location.href = returnUrl
}