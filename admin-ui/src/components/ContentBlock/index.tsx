import React from "react";

interface ContentBlockProps {
    children: React.ReactNode
    title?: React.ReactNode | string,
    showLine?: boolean
}

export default function ({ children, title, showLine = true }: ContentBlockProps) {
    return (
        <div>
            <div className={`text-xl p-3 px-0 font-medium ${showLine && 'border-b'}`}>
                {title}
            </div>
            <div className="pt-4">
                {children}
            </div>
        </div>
    )
}