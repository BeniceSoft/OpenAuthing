import { useParams, history, Navigate, useModel } from "umi";
import ClientForm from "./components/ClientForm";
import ClientDefaultLogo from "@/assets/images/default-client-logo.jpg"
import { useEffect } from "react";
import Spin from "@/components/Spin";
import { Button } from "@/components/ui/button";

export default () => {
    const { id } = useParams();

    if (id === undefined || id === '') {
        return (
            <Navigate to="/admin/clients" replace={true} />
        )
    }

    const { fetch, loading } = useModel('admin.clients.clientDetail')

    useEffect(() => {
        fetch(id)
    }, [])

    return (
        <div className="w-full">
            <Spin spinning={loading}>
                <div className="mb-2">
                    <span onClick={history.back}
                        className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                            <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                        </svg>
                        返回
                    </span>
                </div>
                <div className="flex gap-x-4 items-center mb-4">
                    <div className="flex-1 flex gap-x-4 items-center">
                        <div className="flex-shrink-0 w-16 h-16 rounded overflow-hidden">
                            <img src={ClientDefaultLogo} alt="logo"
                                className="w-full h-full" />
                        </div>
                        <div className="w-full">
                            <h1 className="text-xl font-semibold mb-3">app1</h1>
                            <p className="text-sm text-gray-400">应用显示名称</p>
                        </div>
                    </div>
                    <div className="flex gap-x-4">
                        <Button variant="secondary">
                            数据概览
                        </Button>
                        <Button>
                            体验登录
                        </Button>
                    </div>
                </div>
                <main className="w-full flex flex-col gap-y-6">
                    <ClientForm />
                </main>
            </Spin>
        </div>
    )
}