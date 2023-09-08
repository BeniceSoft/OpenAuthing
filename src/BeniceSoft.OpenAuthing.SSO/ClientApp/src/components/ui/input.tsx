import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const inputVariants = cva(
    "rounded w-full transition-colors disabled:cursor-not-allowed",
    {
        variants: {
            variant: {
                default: 'border-gray-200 focus:border-primary disabled:bg-gray-100',
                solid: 'border-gray-200 bg-gray-100 focus:bg-white focus:border-primary'
            },
            sizeVariant: {
                xs: 'text-xs',
                sm: 'text-sm'
            }
        },
        defaultVariants: {
            variant: 'default',
            sizeVariant: 'sm'
        },
    }
)

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement>, VariantProps<typeof inputVariants> {
    disabled?: boolean
}

const Input = React.forwardRef<HTMLInputElement, InputProps>(
    ({ variant, disabled = false, sizeVariant, ...props }, ref) => {

        return (
            <input
                className={cn(inputVariants({ variant, sizeVariant }))}
                ref={ref}
                disabled={disabled}
                {...props}
            />
        )
    }
)
Input.displayName = "Input"


const inputLabelVariants = cva(
    "text-sm flex",
    {
        variants: {
            variant: {
            },
        },
        defaultVariants: {
        },
    }
)

interface inputLabelProps extends React.InputHTMLAttributes<HTMLLabelElement>, VariantProps<typeof inputLabelVariants> {
    text?: string
    required?: boolean

}

const InputLabel: React.FC<inputLabelProps> = (
    ({ variant, className, children, text, required }) => {

        return (
            <label className={cn(inputVariants({ variant, className }))}>
                <span className={cn("block mb-1", required && "after:content-['*'] after:text-red-600")}>{text}</span>
                <div className="flex-1">
                    {children}
                </div>
            </label>
        )
    }
)

export { Input, inputVariants, InputLabel, inputLabelVariants }
