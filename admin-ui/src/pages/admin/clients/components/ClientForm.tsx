import ContentBlock from "@/components/ContentBlock"
import { Listbox, Transition } from "@headlessui/react"
import { CheckIcon, ChevronUpDownIcon } from "@heroicons/react/24/outline"
import { Fragment, useState } from "react"
import { useForm } from "react-hook-form"
import ClientDefaultLogo from "@/assets/images/default-client-logo.jpg"

const clientTypes = [{
    value: 'confidential',
    label: '私密'
}, {
    value: 'public',
    label: '公开'
}]

export default () => {
    const { register, formState: { errors, isValid }, handleSubmit } = useForm()
    const [clientType, setClientType] = useState(clientTypes[0])

    const onLogFileChange = (file: any) => {
        console.log(file)
    }

    const onSubmit = (data: any) => {
        console.log(data)
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <ContentBlock title="基本信息" showLine={false}>
                <div className="grid grid-cols-2 gap-x-16">
                    <div className="col-span-1">
                        <label className="pb-8 flex flex-col gap-y-2 relative">
                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                应用 Id
                            </span>
                            <input type="text"
                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                                aria-invalid={errors.clientId ? 'true' : 'false'}
                                {...register('clientId', { required: true })} />
                            {errors.clientId &&
                                <span className="absolute bottom-1.5 left-0 text-sm text-red-500">
                                    请输入「应用 Id」
                                </span>
                            }
                        </label>
                        <label className="pb-8 flex flex-col gap-y-2 relative">
                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                应用名称
                            </span>
                            <input type="text"
                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                                aria-invalid={errors.displayName ? 'true' : 'false'}
                                {...register('displayName', { required: true })} />
                            {errors.displayName &&
                                <span className="absolute bottom-1.5 left-0 text-sm text-red-500">
                                    请输入「应用名称」
                                </span>
                            }
                        </label>
                    </div>
                    <div className="col-span-1">
                        <div className="flex flex-col gap-y-2">
                            <span className="text-gray-600 dark:text-gray-400 text-sm">
                                应用图标
                            </span>
                            <div className="w-28 h-28 rounded overflow-hidden relative group">
                                <img className="w-full h-full"
                                    src={ClientDefaultLogo} alt="logo" />
                                <div className="flex absolute w-full h-full top-0 left-0 bg-gray-600 bg-opacity-80 opacity-0 group-hover:opacity-100 items-center justify-center transition-opacity duration-500">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 stroke-white">
                                        <path strokeLinecap="round" strokeLinejoin="round" d="M2.25 15.75l5.159-5.159a2.25 2.25 0 013.182 0l5.159 5.159m-1.5-1.5l1.409-1.409a2.25 2.25 0 013.182 0l2.909 2.909m-18 3.75h16.5a1.5 1.5 0 001.5-1.5V6a1.5 1.5 0 00-1.5-1.5H3.75A1.5 1.5 0 002.25 6v12a1.5 1.5 0 001.5 1.5zm10.5-11.25h.008v.008h-.008V8.25zm.375 0a.375.375 0 11-.75 0 .375.375 0 01.75 0z" />
                                    </svg>
                                </div>
                                <input type="file" className="absolute w-full h-full top-0 left-0 opacity-0 cursor-pointer"
                                    onChange={onLogFileChange} />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentBlock>
            <ContentBlock title="协议配置" showLine={false}>
                <div className="grid grid-cols-2 gap-16">
                    <div className="col-span-1 flex flex-col gap-y-8">
                        <label className="block">
                            <span className="text-sm block mb-2 text-gray-600">应用类型</span>
                            <Listbox value={clientType} onChange={setClientType}>
                                <div className="relative mt-1">
                                    <Listbox.Button className="relative w-full cursor-default rounded bg-gray-100 dark:bg-gray-700 py-2 pl-3 pr-10 text-left text-sm focus:outline-none focus-visible:border-indigo-500 focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-orange-300 sm:text-sm">
                                        <span className="block truncate">{clientType.label}</span>
                                        <span className="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-2">
                                            <ChevronUpDownIcon
                                                className="h-5 w-5 text-gray-400"
                                                aria-hidden="true"
                                            />
                                        </span>
                                    </Listbox.Button>
                                    <Transition
                                        as={Fragment}
                                        leave="transition ease-in duration-100"
                                        leaveFrom="opacity-100"
                                        leaveTo="opacity-0"
                                    >
                                        <Listbox.Options className="absolute mt-1 max-h-60 w-full overflow-auto rounded-md bg-white dark:bg-gray-700  py-1 text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm">
                                            {clientTypes.map((clientType, idx) => (
                                                <Listbox.Option
                                                    key={clientType.value}
                                                    className={({ active }) =>
                                                        `relative cursor-default select-none py-2 pl-10 pr-4 ${active ? 'bg-blue-100 dark:bg-gray-400 text-blue-600' : 'text-gray-900'}`
                                                    }
                                                    value={clientType}
                                                >
                                                    {({ selected }) => (
                                                        <>
                                                            <span
                                                                className={`block truncate ${selected ? 'font-medium text-blue-600' : 'font-normal'}`}>
                                                                {clientType.label}
                                                            </span>
                                                            {selected ? (
                                                                <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-blue-600">
                                                                    <CheckIcon className="h-5 w-5" aria-hidden="true" />
                                                                </span>
                                                            ) : null}
                                                        </>
                                                    )}
                                                </Listbox.Option>
                                            ))}
                                        </Listbox.Options>
                                    </Transition>
                                </div>
                            </Listbox>
                        </label>
                        <label className="block">
                            <span className="text-sm block mb-2 text-gray-600">应用机密</span>
                            <input type="password" autoComplete="off"
                                className="text-sm rounded border-none bg-gray-100 dark:bg-gray-700 w-full"
                                {...register('clientSecret')} />
                        </label>
                        <label className="block">
                            <span className="text-sm block mb-2 text-gray-600">应用地址</span>
                            <input type="text"
                                className="text-sm rounded border-none bg-gray-100 dark:bg-gray-700 w-full" />
                        </label>
                    </div>
                    <div className="col-span-1 flex flex-col gap-y-8">
                    </div>
                </div>
            </ContentBlock>
            <div className="mt-6">
                <button type="submit"
                    className="border-none text-white bg-blue-600 rounded px-6 py-1.5 text-sm hover:bg-blue-800 transition-colors"
                    disabled={!isValid}>
                    保存
                </button>
            </div>
        </form>
    )
}