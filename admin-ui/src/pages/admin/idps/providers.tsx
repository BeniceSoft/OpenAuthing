import { Dispatch, connect, history, useModel } from "umi"
import PageHeader from "../components/PageHeader"
import { IdpTemplatesModelState } from "@/models/idpTemplates"
import { useEffect } from "react"
import Spin from "@/components/Spin"

interface IdentityProvidersPageProps {
    templates?: []
    loading?: boolean
    dispatch: Dispatch
}

const IdentityProvidersPage = (props: IdentityProvidersPageProps) => {

    const { dispatch, templates, loading } = props

    useEffect(() => {
        dispatch({
            type: 'idpTemplates/fetch'
        })
    }, [])


    const onItemClick = (providerKey: string) => {
        history.push(`/admin/idps/create/${providerKey}`)
    }

    const onSearch = (e: any) => {
    }

    return (
        <div className="w-full h-full flex flex-col">
            <div className="mb-2">
                <span onClick={history.back}
                    className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                    </svg>
                    返回
                </span>
            </div>
            <PageHeader title="选择身份源" />
            <div className="py-2 mb-6 px-2 transition duration-150 rounded bg-gray-50 flex items-center border border-transparent focus-within:bg-white focus-within:border-blue-600 focus-within:hover:bg-transparent hover:bg-gray-100 dark:bg-gray-800">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor"
                    className="w-4 h-4 flex-initial stroke-gray-600">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z" />
                </svg>
                <input type="text"
                    placeholder="输入身份源名称查询"
                    className="flex-1 border-none bg-transparent text-sm pl-2 py-0 inline-block focus:outline-none focus:ring-0 placeholder-gray-300"
                    onChange={onSearch} />
            </div>

            <Spin spinning={loading ?? false}>
                <div className="mt-2 grid grid-cols-3 xl:grid-cols-4 gap-x-4 gap-y-6">
                    {templates && templates.map((template: any) => (
                        <div key={template.name}
                            onClick={() => onItemClick(template.name)}
                            className="flex p-8 gap-x-4 rounded bg-gray-50 overflow-hidden h-28 cursor-pointer transition-[translate_shadow] hover:-translate-y-2 hover:shadow-[0_6px_12px_0_#eceef4]">
                            <div className="flex-1 bg-white">
                                <img className="w-12 h-12"
                                    src={template.logo} />
                            </div>
                            <div className="w-[calc(100%-64px)]">
                                <h3 className="text-gray-900 mb-1">{template.title}</h3>
                                <p className="flex-1 w-full text-xs text-gray-400 leading-relaxed overflow-ellipsis line-clamp-2"
                                    title={template.description}>
                                    {template.description}
                                </p>
                            </div>
                        </div>
                    ))}
                </div>
            </Spin>

        </div>
    )
}

export default connect(({ idpTemplates, loading }: { loading: any, idpTemplates: IdpTemplatesModelState }) => ({
    loading: loading.effects['idpTemplates/fetch'],
    ...idpTemplates
}))(IdentityProvidersPage)