
import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const badgeVariants = cva(
    "outline-block rounded",
    {
        variants: {
            variant: {
                default: "bg-primary/20 text-primary",
                destructive: "bg-destructive/20 text-destructive",
                violet: "bg-violet-200 text-violet-800",
                ghost: ''
            },
            size: {
                xs: 'text-xs px-1 py-0.5',
                sm: 'text-xs px-2 py-1'
            }
        },
        defaultVariants: {
            variant: "default",
            size: 'sm'
        },
    }
)

export interface BadgeProps
    extends React.HTMLAttributes<HTMLSpanElement>,
    VariantProps<typeof badgeVariants> { }

function Badge({ className, variant, size, ...props }: BadgeProps) {
    return (
        <span className={cn(badgeVariants({ variant, size }), className)} {...props} />
    )
}

export { Badge, badgeVariants }
