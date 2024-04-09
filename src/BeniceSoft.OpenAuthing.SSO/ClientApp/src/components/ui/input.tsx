import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"

const inputVariants = cva(
    "block w-full border-gray-200 rounded-md text-sm text-gray-800 transition-colors disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600",
    {
        variants: {
            variant: {
                default: 'border-gray-200 focus:border-primary ring:border-primary disabled:bg-gray-100',
                solid: 'border-gray-200 bg-gray-100 focus:bg-white focus:border-primary'
            },
            sizeVariant: {
                default: 'py-2 px-3',
            }
        },
        defaultVariants: {
            variant: 'default',
            sizeVariant: 'default'
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

interface InputLabelProps extends React.InputHTMLAttributes<HTMLLabelElement>, VariantProps<typeof inputLabelVariants> {
    text?: string
    required?: boolean
    errorMessage?: string
}

const InputLabel: React.FC<InputLabelProps> = (
    ({ className, children, text, required, errorMessage }) => {

        return (
            <label className={cn("block text-sm font-medium text-gray-500 dark:text-gray-300", className)}>
                <span className={cn("block mb-1", required && "after:content-['*'] after:text-red-600")}>{text}</span>
                <div className="flex-1">
                    {children}
                </div>
                {errorMessage &&
                    <p className="text-xs text-destructive">{errorMessage}</p>
                }
            </label>
        )
    }
)

export { Input, inputVariants, InputLabel, inputLabelVariants }
