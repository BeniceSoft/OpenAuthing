import { useState, useEffect, FC, ReactElement } from 'react'
import * as ReactDOMClient from 'react-dom/client';
import { TransitionGroup } from 'react-transition-group'
import Transition from '@/components/Transition'

export type MessageType = 'info' | 'success' | 'danger' | 'warning'

export interface MessageProps {
    text: string;
    type: MessageType
}

export const Message: FC<MessageProps> = (props: MessageProps) => {
    const { text, type } = props

    const renderIcon = (messageType: MessageType): ReactElement => {
        switch (messageType) {
            case 'success':
                return (
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor"
                        className="w-6 h-6 stroke-[#52c41a]">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 12.75l6 6 9-13.5" />
                    </svg>)
            case 'danger':
                return (
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor"
                        className="w-6 h-6 stroke-[#ff4d4f]">
                        <path strokeLinecap="round" strokeLinejoin="round"
                            d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z" />
                    </svg>
                )
            case 'warning':
                return (
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor"
                        className="w-6 h-6 stroke-[#faad14]">
                        <path strokeLinecap="round" strokeLinejoin="round"
                            d="M12 9v3.75m9-.75a9 9 0 11-18 0 9 9 0 0118 0zm-9 3.75h.008v.008H12v-.008z" />
                    </svg>
                )
            case 'info':
            default:
                return (
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor"
                        className="w-6 h-6 stroke-[#1677ff]">
                        <path strokeLinecap="round" strokeLinejoin="round"
                            d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z" />
                    </svg>
                )
        }
    }

    return (
        <div className="text-center mt-2 pointer-events-none">
            <div className="bg-white p-2 px-4 rounded-md text-center inline-flex text-sm shadow-md max-w-md items-center">
                <div className="mr-2 items-start">
                    {renderIcon(type)}
                </div>
                <div className="text-slate-600">
                    {text}
                </div>
            </div>
        </div>
    )
}

export interface MessageApi {
    info: (text: string, duration?: number) => void;
    success: (text: string, duration?: number) => void;
    warning: (text: string, duration?: number) => void;
    error: (text: string, duration?: number) => void;
}

export interface Notice {
    text: string;
    key: string;
    type: MessageType;
    duration?: number
}

let seed = 0
const now = Date.now()
const getUuid = (): string => {
    const id = seed
    seed += 1
    return `MESSAGE_${now}_${id}`
}

let add: (notice: Notice) => void

export const MessageContainer = () => {
    const [notices, setNotices] = useState<Notice[]>([])
    const maxCount = 10

    const remove = (notice: Notice) => {
        const { key } = notice

        setNotices((prevNotices) => (
            prevNotices.filter(({ key: itemKey }) => key !== itemKey)
        ))
    }

    add = (notice: Notice) => {
        setNotices((prevNotices) => [...prevNotices, notice])

        setTimeout(() => {
            remove(notice)
        }, notice.duration || 3 * 1000)
    }

    useEffect(() => {
        if (notices.length > maxCount) {
            const [firstNotice] = notices
            remove(firstNotice)
        }
    }, [notices])

    return (
        <div className="pointer-events-none fixed top-0 left-0 w-screen z-[1050]">
            <TransitionGroup>
                {
                    notices.map(({ text, key, type }) => (
                        <Transition
                            timeout={200}
                            in
                            animation="slide-in-top"
                            key={key}
                        >
                            <Message type={type} text={text} />
                        </Transition>
                    ))
                }
            </TransitionGroup>
        </div>
    )
}

let el = document.querySelector('#message-wrapper')
if (!el) {
    el = document.createElement('div')
    el.className = 'message-wrapper'
    el.id = 'message-wrapper'
    document.body.append(el)
}
const root = ReactDOMClient.createRoot(el);
root.render(<MessageContainer />)


const api: MessageApi = {
    info: (text, duration) => {
        add({
            text,
            key: getUuid(),
            type: 'info',
            duration
        })
    },
    success: (text, duration) => {
        add({
            text,
            key: getUuid(),
            type: 'success',
            duration
        })
    },
    warning: (text, duration) => {
        add({
            text,
            key: getUuid(),
            type: 'warning',
            duration
        })
    },
    error: (text, duration) => {
        add({
            text,
            key: getUuid(),
            type: 'danger',
            duration
        })
    }
}

export default api