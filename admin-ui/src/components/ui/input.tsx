import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const inputVariants = cva(
    "rounded w-full transition-colors disabled:cursor-not-allowed",
    {
        variants: {
            variant: {
                default: 'border-gray-200 focus:ring-0 focus:border-primary disabled:bg-gray-100 placeholder:text-gray-400 aria-invalid:border-destructive',
                solid: 'border-gray-200 bg-gray-100 focus:ring-0 focus:bg-white focus:border-primary aria-invalid:border-destructive'
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
    disabled?: boolean,
    invalid?: boolean
}

const Input = React.forwardRef<HTMLInputElement, InputProps>(
    ({ variant, disabled = false, className, sizeVariant, invalid, ...props }, ref) => {

        return (
            <input
                aria-invalid={invalid}
                className={cn(inputVariants({ variant, sizeVariant, className }))}
                ref={ref}
                disabled={disabled}
                type="text"
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

type InputErrorMessageProps = {
    message?: string
}

const InputErrorMessage: React.FC<InputErrorMessageProps> = ({ message }) => {
    return (
        <span className="text-xs text-destructive">{message}</span>
    )
}

export { Input, inputVariants, InputLabel, inputLabelVariants, InputErrorMessage }
