import { cn } from "@/lib/utils";
import { Dialog, Transition } from "@headlessui/react";
import React, { Fragment, useState } from "react";
import * as ReactDOMClient from 'react-dom/client';

type ModalButtonSetting = {
    text?: string | ((isBusy: boolean) => React.ReactNode)
    className?: string
}

interface ConfirmModalArg {
    title: React.ReactNode
    content: React.ReactNode
    onOK?: () => Promise<void> | void
    onClose?: () => void
    okButton?: ModalButtonSetting
    closeButton?: ModalButtonSetting
}

interface ConfirmModalProps extends ConfirmModalArg {
    isOpen?: boolean
}

export default function ConfirmModal({
    isOpen: opened,
    title,
    content,
    onOK = () => { },
    onClose = () => { },
    okButton = {},
    closeButton = {}
}: ConfirmModalProps) {
    const [isOpen, setIsOpen] = useState(opened);
    const [isBusy, setBusy] = useState<boolean>(false)

    const { text: okText, className: okClassName } = okButton
    const { text: closeText, className: closeClassName } = closeButton

    const handleClose = () => {
        onClose()
        setIsOpen(false);
    };

    const handleConfirm = async () => {
        try {
            setBusy(true)
            await onOK();
        } finally {
            setBusy(false)
            setIsOpen(false);
        }
    };

    return (
        <Transition appear show={isOpen} as={Fragment}>
            <Dialog as="div" className="relative z-[999]" onClose={handleClose}>
                <Transition.Child
                    as={Fragment}
                    enter="ease-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in duration-200"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                >
                    <div className="fixed inset-0 bg-background/80 backdrop-blur" />
                </Transition.Child>

                <div className="fixed inset-0 overflow-y-auto">
                    <div className="flex min-h-full items-center justify-center p-0 text-center">
                        <Transition.Child
                            as={Fragment}
                            enter="ease-out duration-300"
                            enterFrom="opacity-0 scale-95"
                            enterTo="opacity-100 scale-100"
                            leave="ease-in duration-200"
                            leaveFrom="opacity-100 scale-100"
                            leaveTo="opacity-0 scale-95"
                        >
                            <Dialog.Panel className="w-full max-w-md min-w-md transform overflow-hidden rounded-lg border bg-background p-6 shadow-lg dark:bg-slate-700 text-left align-middle transition-all py-5 px-6">
                                <h3 className="text-lg font-medium leading-6 text-gray-900 dark:text-gray-300">
                                    {title}
                                </h3>
                                <div className="mt-2 text-sm text-gray-500">
                                    {content}
                                </div>

                                <div className="flex mt-4 justify-end gap-x-4">
                                    <button
                                        type="button"
                                        disabled={isBusy}
                                        className={cn("inline-flex justify-center px-4 py-1.5 text-sm text-white bg-red-600 border border-transparent rounded-md disabled:cursor-not-allowed disabled:bg-opacity-30", okClassName)}
                                        onClick={handleConfirm}
                                    >
                                        {okText ?
                                            (typeof okText === 'function' ? okText(isBusy) : okText) :
                                            (isBusy ? '处理中' : '确定')
                                        }
                                    </button>
                                    <button
                                        type="button"
                                        disabled={isBusy}
                                        className={cn("inline-flex justify-center px-4 py-1.5 text-sm text-gray-700 bg-gray-100 border border-transparent rounded-md", closeClassName)}
                                        onClick={handleClose}
                                    >
                                        {closeText ?
                                            (typeof closeText === 'function' ? closeText(isBusy) : closeText) :
                                            '取消'
                                        }
                                    </button>
                                </div>
                            </Dialog.Panel>
                        </Transition.Child>
                    </div>
                </div>
            </Dialog >
        </Transition >
    );
}

let el = document.querySelector('#modal-wrapper')
if (!el) {
    el = document.createElement('div')
    el.className = 'modal-wrapper'
    el.id = 'modal-wrapper'
    document.body.append(el)
}

ConfirmModal.confirm = (props: ConfirmModalProps) => {
    const div = document.createElement('div');

    div.setAttribute('id', 'confirm-modal');
    el?.appendChild(div);

    const root = ReactDOMClient.createRoot(div);

    const handleDestroy = () => {
        setTimeout(() => {
            root.unmount()
            div.remove();
        }, 1000);
    };
    root.render(<ConfirmModal
        {...props}
        isOpen={true}
        onClose={handleDestroy}
    />)
};

export function confirm(config: ConfirmModalArg) {
    ConfirmModal.confirm(config);
}