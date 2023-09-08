import React from "react"
import WaitingAnimationData from '@/assets/animations/waiting.json'
import Lottie from "lottie-react"

export interface SpinProps {
    spinning: boolean
    children?: React.ReactNode,
    iconStyle?: 'svg' | 'lottie',
    spingingText?: React.ReactNode
}

export default (props: SpinProps) => {
    const { spinning = true, iconStyle, spingingText } = props


    if (spinning) {
        return (
            <div className="flex flex-col justify-center items-center py-8">
                {iconStyle === 'lottie' ?
                    <Lottie animationData={WaitingAnimationData} className="w-64 h-64" /> :
                    <div className="animate-spin inline-block mb-4">
                        <div className="grid grid-cols-2 grid-rows-2 gap-x-1 gap-y-1">
                            <i className="rounded w-2 h-2 bg-blue-200" />
                            <i className="rounded w-2 h-2 bg-blue-300" />
                            <i className="rounded w-2 h-2 bg-blue-400" />
                            <i className="rounded w-2 h-2 bg-blue-500" />
                        </div>
                    </div>
                }
                {spingingText &&
                    <div className="text-center text-gray-400 text-sm">{spingingText}</div>
                }
            </div>
        )
    }

    return (
        <>{props.children}</>
    )
}