import { Dispatch, Link, connect, history } from "umi"
import PageHeader from "../components/PageHeader"
import React, { useEffect } from "react"
import { IdPsModelState } from "@/models/idps"
import { ExternalIdentityProvider } from "@/@types/identityProvider"
import Spin from "@/components/Spin"
import Empty from "@/components/Empty"
import { Switch } from "@headlessui/react"
import classNames from "classnames"

interface IdPsPageProps {
    loading?: boolean
    idps?: ExternalIdentityProvider[]
    dispatch: Dispatch
}

const IdPsPage: React.FC<IdPsPageProps> = ({
    dispatch,
    loading,
    idps
}: IdPsPageProps) => {

    useEffect(() => {
        dispatch({ type: 'idps/fetch' })

        return () => {
            dispatch({ type: 'idps/clear' })
        }
    }, [])

    return (
        <div className="w-full h-full overflow-y-hidden flex flex-col">
            <PageHeader title="身份提供程序" description="对接第三方，使你的应用支持使用该方式进行认证、授权登录。"
                rightRender={() => (
                    <button onClick={() => history.push('/admin/idps/providers')}
                        className="rounded bg-blue-600 text-white text-sm px-4 py-1.5 hover:bg-blue-700 transition-colors">
                        添加身份提供程序
                    </button>
                )} />

            <div className="flex-1 overflow-auto">
                <Spin spinning={loading ?? false}>
                    <Empty isEmpty={(idps?.length ?? 0) == 0}>
                        <div className="flex flex-col gap-4">
                            {idps && idps!.map(idp => (
                                <div key={idp.id}
                                    className="p-4 rounded transition-colors bg-gray-50/50 w-full flex space-x-2 items-center cursor-pointer hover:bg-gray-100">
                                    <div>
                                        <img className="w-12 h-12"
                                            src={idp.logo}
                                            alt="logo" />
                                    </div>
                                    <div className="flex-1">
                                        <div className="flex items-center">
                                            <h3 className="font-medium">{idp.displayName}</h3>
                                            <span className="text-xs rounded py-0.5 px-1 ml-2 text-blue-600 border border-blue-600">{idp.title}</span>
                                        </div>
                                        <p className="text-gray-400 text-sm">{idp.name}</p>
                                    </div>
                                    <div className="flex gap-x-2 items-center">
                                        <Switch
                                            checked={idp.enabled}
                                            className={classNames(idp.enabled ? 'bg-blue-600' : 'bg-gray-200',
                                                "relative inline-flex items-center border-none h-7 w-14 shrink-0 cursor-pointer rounded-full transition-colors duration-200 ease-in-out",
                                                "focus:outline-none")}>
                                            <span
                                                aria-hidden="true"
                                                className={classNames(idp.enabled ? 'translate-x-7' : 'translate-x-0',
                                                    "pointer-events-none inline-block h-7 w-7 transform rounded-full bg-white shadow-lg ring-0 transition duration-200 ease-in-out")}>
                                            </span>
                                        </Switch>
                                        <span className={classNames(
                                            "inline-block text-sm",
                                            idp.enabled ? 'text-gray-600' : 'text-red-600'
                                        )}>
                                            {idp.enabled ? '已启用' : '已禁用'}
                                        </span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </Empty>
                </Spin>
            </div>
        </div >
    )
}

export default connect(({ idps, loading }: { loading: any, idps: IdPsModelState }) => ({
    loading: loading.effects['idps/fetch'],
    ...idps
}))(IdPsPage)