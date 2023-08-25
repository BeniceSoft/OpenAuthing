import { Backend, Device, IoT, Spa, Web } from "@/components/Icons"
import { Dialog, RadioGroup, Transition } from "@headlessui/react"
import { CheckIcon } from "@heroicons/react/24/solid"
import classNames from "classnames"
import { Fragment, useState } from "react"
import { useForm } from "react-hook-form"

const ClientTypes = [{
    key: 'standard_web',
    title: '标准 Web 应用',
    description: '多页面并支持跳转的网页应用',
    icon: <Web />
}, {
    key: 'spa',
    title: '单页应用程序',
    description: '只有单个页面的纯前端网页应用',
    icon: <Spa />
}, {
    key: 'native',
    title: '客户端应用',
    description: '在手机、桌面和其他智能设备上运行的本地应用',
    icon: <Device />
}, {
    key: 'backend',
    title: '后端应用',
    description: '无前端界面，只提供后端服务的应用',
    icon: <Backend />
}, {
    key: 'iot',
    title: 'IoT 应用',
    description: '物联网应用程序或其他无浏览器或输入受限的设备',
    icon: <IoT />
}]

export default ({
    isOpen,
    isCreating,
    onClose,
    onCreate
}: { isOpen: boolean, isCreating?: boolean, onClose: () => void, onCreate?: (value: any) => Promise<boolean>; }) => {
    const { register, handleSubmit, reset, formState: { errors } } = useForm()
    const [clientType, setClientType] = useState(ClientTypes[0])

    const restValues = () => {
        reset()
        setClientType(ClientTypes[0])
    }

    const onSubmit = async (value: any) => {
        if (onCreate && await onCreate({
            ...value,
            clientType: clientType.key
        })) {
            restValues()
        }
    }

    const handleClose = () => {
        onClose()

        restValues()
    }

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
                    <div className="fixed inset-0 bg-black bg-opacity-25" />
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
                            <Dialog.Panel className="w-full max-w-4xl min-w-4xl transform overflow-hidden rounded-xl bg-white dark:bg-slate-700 text-left align-middle shadow-xl transition-all">
                                <Dialog.Title
                                    as="h3"
                                    className="text-lg px-8 py-4 font-medium leading-6 text-gray-900 dark:text-gray-300 border-b dark:border-gray-500"
                                >
                                    创建应用
                                </Dialog.Title>
                                <form onSubmit={handleSubmit(onSubmit)}>
                                    <div className="py-6 px-8 flex flex-col w-full">
                                        <label className="pb-8 flex flex-col gap-y-2 relative">
                                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                                应用标识
                                            </span>
                                            <input type="text"
                                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm focus:bg-white transition duration-300 aria-invalid:ring-red-500 placeholder:text-gray-300"
                                                placeholder="请输入应用标识（要求唯一）"
                                                disabled={isCreating}
                                                aria-invalid={errors.clientId ? 'true' : 'false'}
                                                {...register('clientId', { required: true })} />
                                        </label>
                                        <label className="pb-8 flex flex-col gap-y-2 relative">
                                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                                应用名称
                                            </span>
                                            <input type="text"
                                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm focus:bg-white transition duration-300 aria-invalid:ring-red-500 placeholder:text-gray-300"
                                                placeholder="请输入应用显示名称"
                                                disabled={isCreating}
                                                aria-invalid={errors.displayName ? 'true' : 'false'}
                                                {...register('displayName', { required: true })} />
                                        </label>
                                        <RadioGroup value={clientType} onChange={setClientType} disabled={isCreating}>
                                            <RadioGroup.Label className="block text-sm mb-2 text-gray-600 dark:text-gray-400 after:content-['*'] after:ml-0.5 after:text-red-500">
                                                应用类型
                                            </RadioGroup.Label>
                                            <div className="flex w-full overflow-x-auto gap-x-6 justify-between">
                                                {ClientTypes.map((clientType) => (
                                                    <RadioGroup.Option
                                                        key={clientType.key}
                                                        value={clientType}
                                                        disabled={isCreating}
                                                        className={({ active, checked }) => classNames(
                                                            "h-72 w-36 min-w-36 px-3 py-6 border-2 rounded cursor-pointer transition-colors duration-300",
                                                            "disabled:cursor-not-allowed",
                                                            isCreating ? "" : "hover:border-blue-600 hover:bg-blue-50/50",
                                                            checked ? "border-blue-600 bg-blue-50/50" : "border-gray-50 bg-gray-50"
                                                        )}
                                                    >
                                                        {({ checked }) => (
                                                            <>
                                                                <div className="flex w-full h-full justify-center items-center">
                                                                    <div className="flex flex-col items-center justify-start w-full h-full gap-y-2">
                                                                        <RadioGroup.Label
                                                                            as="div"
                                                                            className={classNames("flex flex-col items-center gap-y-6")}
                                                                        >
                                                                            <div className="w-12 h-10 bg-blue-600 rounded flex items-center justify-center">
                                                                                {clientType.icon}
                                                                            </div>
                                                                            <h3 className="text-sm font-medium text-gray-700">{clientType.title}</h3>
                                                                        </RadioGroup.Label>
                                                                        <RadioGroup.Description
                                                                            as="div"
                                                                            className={classNames("flex-1 flex flex-col items-center justify-between text-sm text-gray-400")}
                                                                        >
                                                                            <div className="text-center leading-6">{clientType.description}</div>
                                                                            <div className="">
                                                                                <Transition show={checked}
                                                                                    as={Fragment}
                                                                                    enter="ease-out duration-300"
                                                                                    enterFrom="opacity-0"
                                                                                    enterTo="opacity-100"
                                                                                    leave="ease-in duration-200"
                                                                                    leaveFrom="opacity-100"
                                                                                    leaveTo="opacity-0">
                                                                                    <div className="shrink-0 p-1 rounded-full bg-blue-600">
                                                                                        <CheckIcon className="h-4 w-4 stroke-2 stroke-white" />
                                                                                    </div>
                                                                                </Transition>
                                                                            </div>
                                                                        </RadioGroup.Description>
                                                                    </div>

                                                                </div>
                                                            </>
                                                        )}
                                                    </RadioGroup.Option>
                                                ))}
                                            </div>
                                        </RadioGroup>
                                    </div>

                                    <div className="py-4 px-6 flex gap-x-4 items-center justify-end">
                                        <button
                                            type="button"
                                            className="justify-center rounded-md border border-transparent bg-gray-200 px-5 py-1.5 text-sm text-gray-600 hover:bg-gray-300 focus:outline-none transition-colors disabled:bg-gray-100 disabled:cursor-not-allowed"
                                            onClick={handleClose}
                                            disabled={isCreating}
                                        >
                                            取消
                                        </button>
                                        <button
                                            type="submit"
                                            className="justify-center rounded-md border border-transparent bg-blue-600 px-5 py-1.5 text-sm text-white hover:bg-blue-700 focus:outline-none transition-colors  disabled:bg-blue-400 disabled:cursor-not-allowed"
                                            disabled={isCreating}
                                        >
                                            创建
                                        </button>
                                    </div>

                                </form>
                            </Dialog.Panel>
                        </Transition.Child>
                    </div>
                </div>
            </Dialog >
        </Transition >
    )
}