import { Link, connect, history, useDispatch } from "umi"
import PageHeader from "../components/PageHeader"
import Empty from "@/components/Empty"
import { Fragment, useCallback, useEffect, useState } from "react"
import { ClientsModelState } from "@/models/clients"
import { Client } from "@/@types/openiddict"
import Spin from "@/components/Spin"
import CreateClientDialog from "./components/CreateClientDialog"
import ClientDefaultLogo from "@/assets/images/default-client-logo.jpg"
import { debounce } from 'lodash'
import ApplicationService from "@/services/application.service"
import Message from "@/components/Message"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"

const ClientsPage = ({
    isLoading,
    clients
}: { isLoading: boolean, clients?: Client[] }) => {
    const [isCreateDialogOpened, setCreateDialogOpened] = useState(false)
    const [isCreating, setCreating] = useState(false)
    const [searchKey, setSearchKey] = useState('')

    const dispatch = useDispatch()
    const isEmpty = typeof clients === undefined || clients?.length == 0

    const doSearch = (searchKey: string = '') => {
        dispatch({
            type: 'clients/fetchAllClients',
            payload: {
                searchKey
            }
        })
    }

    const search = useCallback(debounce(doSearch, 500), [])

    useEffect(() => {
        doSearch()

        return () => {
            dispatch({
                type: 'clients/clear'
            })
        }
    }, [])

    const onClickClient = (clientId: string) => {
        history.push(`/admin/clients/${clientId}`)
    }

    const onCreate = async (value: any) => {
        setCreating(true)
        try {
            const id = await ApplicationService.create(value)
            Message.success('应用创建成功')

            return true
        } catch {
            Message.success('应用创建失败')
            return false
        } finally {
            setCreateDialogOpened(false)
            setCreating(false)

            doSearch()
        }
    }

    const onSearchKeyChange = (event: any) => {
        const inputValue = event.target.value;
        setSearchKey(inputValue);

        search(inputValue)
    }

    return (
        <>
            <div className="w-full h-full">
                <PageHeader title="应用"
                    description="管理应用资源，进行身份验证"
                    rightRender={() => (
                        <Button onClick={() => setCreateDialogOpened(true)}>
                            创建应用
                        </Button>
                    )} />
                <div className="py-2 mb-4 sticky -top-8 z-10 bg-white dark:bg-slate-900">
                    <div className="w-[380px] py-1.5 px-2 transition duration-150 rounded bg-gray-50 flex items-center border border-transparent focus-within:bg-white focus-within:border-blue-600 focus-within:hover:bg-transparent hover:bg-gray-100 dark:bg-gray-800">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor"
                            className="w-4 h-4 flex-initial stroke-gray-600">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z" />
                        </svg>
                        <input type="text"
                            placeholder="输入关键字查询"
                            className="flex-1 border-none bg-transparent text-sm pl-2 py-0 inline-block focus:outline-none focus:ring-0 placeholder-gray-300"
                            value={searchKey}
                            onChange={onSearchKeyChange} />
                    </div>
                </div>

                <Spin spinning={isLoading} iconStyle="lottie" spingingText="正在拉取应用列表中...">
                    <Empty isEmpty={isEmpty} description="应用列表为空">
                        <div className="grid grid-cols-3 gap-8 pb-8">
                            {clients?.map(client => (
                                <div key={client.id}
                                    className="relative group flex flex-col rounded-md bg-gray-50 dark:bg-gray-800 cursor-pointer transition duration-300 hover:shadow-2md dark:hover:shadow-none">
                                    <div className="flex w-full p-6"
                                        onClick={() => onClickClient(client.id)}>
                                        <div className="flex-none">
                                            <Avatar className="rounded w-14 h-14">
                                                <AvatarImage src={client.logoUrl} />
                                                <AvatarFallback>
                                                    <img src={ClientDefaultLogo} />
                                                </AvatarFallback>
                                            </Avatar>
                                        </div>
                                        <div className="flex-1 pl-4 -ml-2 overflow-hidden">
                                            <h3 className="text-xl font-normal mb-1 max-w-full truncate ... transition-colors duration-300 group-hover:text-blue-600 dark:group-hover:text-gray-300">
                                                {client.displayName}
                                            </h3>
                                            <p className="text-xs text-gray-500  max-w-full truncate ...">
                                                {client.clientId}
                                            </p>
                                        </div>
                                    </div>

                                </div>
                            ))}
                        </div>
                    </Empty>
                </Spin>
            </div>
            <CreateClientDialog isOpen={isCreateDialogOpened} isCreating={isCreating}
                onClose={() => setCreateDialogOpened(false)}
                onCreate={onCreate} />
        </>
    )
}

export default connect(({ loading, clients }: { loading: any, clients: ClientsModelState }) => ({
    clients: clients.clients,
    isLoading: loading.effects['clients/fetchAllClients']
}))(ClientsPage)