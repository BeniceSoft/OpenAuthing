import React from "react";

export default ({
    title,
    description,
    rightRender
}: { title: string, description?: string, rightRender?: () => JSX.Element }) => {
    return (
        <div className="flex gap-y-3 mb-4">
            <div className="flex-1 pr-10 grid gap-y-4">
                <h1 className="text-2xl font-bold">{title}</h1>
                {description &&
                    <p className="text-sm text-gray-400">{description}</p>
                }
            </div>
            <div className="flex-none self-center">
                {rightRender && rightRender()}
            </div>
        </div>
    )
}