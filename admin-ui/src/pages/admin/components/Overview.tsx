import { Listbox, Transition } from "@headlessui/react"
import { Fragment, useState } from "react"

const totalUserFilters = [
    { id: 'all', name: '全部', unavailable: false },
    { id: 'normal', name: '正常', unavailable: false },
    { id: 'disabled', name: '禁用', unavailable: false },
    { id: 'locked', name: '锁定', unavailable: false }
]

const dateRangeFilters = [
    { id: 0, name: '今日', unavailable: false },
    { id: 7, name: '近7天', unavailable: false },
    { id: 30, name: '近30天', unavailable: false },
]

export default () => {
    const [totalUserFilter, setTotalUserFilter] = useState(totalUserFilters[0])
    const [dateRangeFilter, setDateRangeFilter] = useState(dateRangeFilters[0])

    return (
        <>
            <h2 className="font-medium text-xl mb-6">用户数据概览</h2>
            <div className="flex">
                <div className="flex-auto border-r pr-6">
                    <div className="flex justify-between">
                        <div className="flex-auto">
                            <div className="text-sm text-gray-600 leading-[32px]">总用户数</div>
                            <div className="mt-4 text-xl font-medium">
                                2
                            </div>
                        </div>
                        <div className="flex-initial w-[70px]">
                            <Listbox value={totalUserFilter} onChange={setTotalUserFilter}>
                                <Listbox.Button className="relative w-full rounded bg-gray-100 dark:bg-gray-300 text-gray-600 flex justify-between items-center py-1.5 px-2 text-xs focus:outline-none focus-visible:border-indigo-500 focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-opacity-75 focus-visible:ring-offset-2 focus-visible:ring-offset-orange-300 sm:text-sm">
                                    <span className="block truncate">{totalUserFilter.name}</span>
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                                        <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                                    </svg>
                                </Listbox.Button>
                                <Transition
                                    as={Fragment}
                                    leave="transition ease-in duration-100"
                                    leaveFrom="opacity-100"
                                    leaveTo="opacity-0"
                                >
                                    <Listbox.Options className="absolute mt-1 max-h-60 w-[70px] overflow-auto rounded-md bg-white text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm z-10">
                                        {totalUserFilters.map((item) => (
                                            <Listbox.Option
                                                key={item.id}
                                                value={item}
                                                className={({ active }) =>
                                                    `relative select-none py-2 text-center ${active ? 'bg-gray-100' : 'text-gray-900'
                                                    }`
                                                }
                                                disabled={item.unavailable}
                                            >
                                                {item.name}
                                            </Listbox.Option>
                                        ))}
                                    </Listbox.Options>
                                </Transition>
                            </Listbox>
                        </div>
                    </div>
                </div>
                <div className="flex-[0_0_75%] pl-6 flex flex-wrap">
                    <div className="flex-[auto] w-1/3">
                        <div className="text-sm text-gray-600 leading-[32px]">登录用户数</div>
                        <div className="mt-4 text-xl font-medium">
                            2
                        </div>
                    </div>
                    <div className="flex-[auto] w-1/3">
                        <div className="text-sm text-gray-600 leading-[32px]">新增用户数</div>
                        <div className="mt-4 text-xl font-medium">
                            0
                        </div>
                    </div>
                    <div className="flex-[auto] w-1/3">
                        <div className="flex justify-between">
                            <div className="flex flex-col">
                                <div className="text-sm text-gray-600 leading-[32px]">活跃用户数</div>
                                <div className="mt-4 text-xl font-medium">
                                    2
                                </div>
                            </div>
                            <div className="flex-initial w-[80px]">
                                <Listbox value={dateRangeFilter} onChange={setDateRangeFilter}>
                                    <Listbox.Button className="relative w-full rounded bg-gray-100 dark:bg-gray-300 text-gray-600 flex justify-between items-center py-1.5 px-2 text-xs focus:outline-none z-10 sm:text-sm">
                                        <span className="block truncate">{dateRangeFilter.name}</span>
                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                                            <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                                        </svg>
                                    </Listbox.Button>
                                    <Transition
                                        as={Fragment}
                                        leave="transition ease-in duration-100"
                                        leaveFrom="opacity-100"
                                        leaveTo="opacity-0"
                                    >
                                        <Listbox.Options className="absolute mt-1 max-h-60 w-[80px] overflow-auto rounded-md bg-white text-base shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none sm:text-sm z-10">
                                            {dateRangeFilters.map((item) => (
                                                <Listbox.Option
                                                    key={item.id}
                                                    value={item}
                                                    className={({ active }) =>
                                                        `relative select-none py-2 text-center ${active ? 'bg-gray-100' : 'text-gray-900'
                                                        }`
                                                    }
                                                    disabled={item.unavailable}
                                                >
                                                    {item.name}
                                                </Listbox.Option>
                                            ))}
                                        </Listbox.Options>
                                    </Transition>
                                </Listbox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}