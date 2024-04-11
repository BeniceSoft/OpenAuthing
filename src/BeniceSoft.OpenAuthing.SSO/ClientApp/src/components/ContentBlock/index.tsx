import React from "react";

interface ContentBlockProps {
    children: React.ReactNode
    title?: React.ReactNode | string,
    showLine?: boolean
}

export default function ({ children, title, showLine = true }: ContentBlockProps) {
    return (
        <div>
            <div className={`text-xl p-3 px-0 font-semibold text-gray-800 dark:text-gray-200 ${showLine && 'border-b dark:border-b-slate-900'}`}>
                {title}
            </div>
            <div className="pt-4">
                {children}
            </div>
        </div>
    )
}