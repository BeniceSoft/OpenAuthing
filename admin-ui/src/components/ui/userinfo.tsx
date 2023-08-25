import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"
import { Avatar, AvatarFallback, AvatarImage } from "./avatar"
import { User } from "lucide-react"
import { Badge } from "./badge"

export interface UserinfoProps {
    className?: string
    onClick?: () => void
    userInfo: { avatar?: string, nickname: string, userName: string, isSystemBuiltIn: boolean, enabled: boolean }
}

const Userinfo = React.forwardRef<HTMLDivElement, UserinfoProps>(
    ({ className, userInfo, ...props }, ref) => {
        return (
            <div
                className={cn("inline-flex items-center gap-x-3 cursor-pointer", className)}
                ref={ref}
                {...props}>
                <div className="flex items-center gap-x-1">
                    <Avatar className="w-8 h-8">
                        <AvatarImage src={userInfo.avatar}
                            alt="avatar" />
                        <AvatarFallback>
                            <User className={cn("w-5 h-5", "stroke-gray-500")} />
                        </AvatarFallback>
                    </Avatar>
                    <div className="flex-1">
                        <h3 className="text-gray-900 font-medium">{userInfo.nickname}</h3>
                        <p className="text-sm">{userInfo.userName}</p>
                    </div>
                </div>
                {userInfo.isSystemBuiltIn &&
                    <Badge size="xs" variant="violet">系统内置</Badge>
                }
            </div>
        )
    }
)

export { Userinfo }
