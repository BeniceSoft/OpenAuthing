import { useEffect, useState } from "react"
import greetImg from '@/assets/images/greet-bg.png'
import { useOidcUser } from "@/components/oidc/OidcSecure"

const DashboardPage = () => {
    const { oidcUser } = useOidcUser()
    const [greet, setGreet] = useState('')

    useEffect(() => {
        var curHr = new Date().getHours()
        let greet = 'Good evening'

        if (curHr < 12) {
            greet = 'Good morning'
        } else if (curHr < 18) {
            console.log('good afternoon')
            greet = 'Good afternoon'
        }

        setGreet(greet)
    }, [])

    return (
        <div className="flex flex-col w-full gap-y-2 pt-16">
            <div className="w-full rounded-lg px-4 py-4 bg-blue-50 dark:bg-slate-800 flex relative">
                <div className="flex-1 flex flex-col gap-y-4">
                    <h1 className="text-2xl font-bold">数据概览</h1>
                    <p className="font-medium text-gray-600 tracking-wide">
                        Hi,&nbsp;
                        <span className="font-bold text-slate-700">
                            {oidcUser?.nickname ?? '管理员'}
                        </span>
                        <span className="ml-2 text-sm text-slate-500">
                            👏&nbsp;欢迎回来，祝您工作愉快！
                        </span>
                    </p>
                </div>
                <div className="w-64 opacity-80">
                    <img src={greetImg} className="w-64 h-36 absolute bottom-4" />
                </div>
            </div>

        </div>
    )
}

export default DashboardPage