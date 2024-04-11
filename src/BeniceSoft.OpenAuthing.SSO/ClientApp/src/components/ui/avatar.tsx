import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const avatarVariants = cva(
  "border-none overflow-hidden",
  {
    variants: {
      variant: {
        circle: "rounded-full"
      },
      size: {
        default: "size-8",
        xs: "size-7",
        sm: "size-10",
        lg: "size-20"
      }
    },
    defaultVariants: {
      variant: "circle",
      size: "default",
    },
  }
)

export interface AvatarProps extends React.ImgHTMLAttributes<HTMLImageElement>, VariantProps<typeof avatarVariants> {
  src?: string
  alt?: string
  fallback?: string
}

const Avatar = React.forwardRef<HTMLImageElement, AvatarProps>(
  ({ className, variant, size, ...props }, ref) => {

    const handleError: React.ReactEventHandler<HTMLImageElement> = ({ currentTarget }) => {
      const { fallback } = props
      if (fallback) {
        currentTarget.onerror = null; // prevents looping
        currentTarget.src = fallback;
      }
    }



    return (
      <img
        className={cn(avatarVariants({ variant, size, className }))}
        onError={handleError}
        ref={ref}
        {...props}
      />
    )
  }
)
Avatar.displayName = "Avatar"

export { Avatar, avatarVariants }