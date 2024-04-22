import React, { ReactElement, Children, useEffect, useRef } from "react"
import { cva, type VariantProps } from "class-variance-authority"

import { cn } from "@/lib/utils"
import { InfoIcon } from "lucide-react"
import { HSTooltip } from "preline/preline"

const inputVariants = cva(
    "block w-full text-sm disabled:opacity-50 disabled:pointer-events-none",
    {
        variants: {
            variant: {
                default: 'border-gray-200 aria-invalid:border-destructive focus:border-primary aria-invalid:focus:border-destructive aria-invalid:focus:ring-destructive focus:ring-primary dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600',
                solid: 'border-gray-200 bg-gray-100 focus:bg-white focus:border-primary'
            },
            sizeVariant: {
                default: 'py-3 pl-4 pr-5 rounded-lg',
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
    text?: string | ReactElement
    required?: boolean
    errorMessage?: string
    htmlFor?: string
    children?: ReactElement
}

const InputLabel: React.FC<InputLabelProps> = (
    ({ className, children, text, required, errorMessage, htmlFor }) => {
        const tooltipRef = useRef<HTMLDivElement>(null);

        useEffect(() => {
            if (tooltipRef.current) {
                const el = new HSTooltip(tooltipRef.current)
                console.log(el)
            }
        }, [errorMessage])

        const id = htmlFor || getChildId(children);

        return (
            <div className={cn("block", className)}>
                <label htmlFor={id}
                    className={cn(
                        "block text-gray-700 text-sm mb-2 font-medium dark:text-white",
                        required && "after:content-['*'] after:text-red-600",
                        className)}>
                    {text}
                </label>
                <div className="flex-1 relative">
                    {children}

                    {errorMessage &&
                        <div ref={tooltipRef} className="flex absolute h-full items-center top-0 right-1 hs-tooltip [--placement:right]">
                            <InfoIcon className="cursor-pointer w-4 h-4 stroke-destructive/80 hs-tooltip-toggle" />
                            <span className="hs-tooltip-content hs-tooltip-shown:opacity-100 hs-tooltip-shown:visible opacity-0 transition-opacity text-xs rounded-md inline-block absolute invisible z-10 py-1.5 px-2.5 bg-gray-900 text-white" role="tooltip">
                                {errorMessage}
                            </span>
                        </div>
                    }
                </div>
            </div>
        )
    }
)

function getChildId(children?: ReactElement) {
    if (children) {
        const child = Children.only(children);

        if ("id" in child?.props) {
            return child.props.id;
        }
    }
}

export { Input, inputVariants, InputLabel, inputLabelVariants }
