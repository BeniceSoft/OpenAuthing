import LangSelect from "@/components/LangSelect";
import Logo from "@/components/Logo";
import Lottie from "lottie-react";
import { FormattedMessage, Link, Outlet } from "umi";
import animationData from '@/assets/animations/authentication.json'

export default function () {
    return (
        <div className="h-screen w-screen p-0 flex relative">
            <div className="lg:hidden absolute top-0 w-full py-8 px-5">
                <Logo className="w-48"/>
            </div>
            <div className="py-6 px-8 hidden lg:flex w-1/4 min-w-[428px] max-w-2xl bg-gray-100 justify-between flex-col dark:bg-neutral-900">
                <div className="flex justify-between items-center">
                    <Link to="/"><Logo className="w-48" /></Link>
                    <LangSelect />
                </div>
                <div className="mx-auto space-y-5">
                    <p className="text-xl font-medium text-gray-700 dark:text-gray-200"><FormattedMessage id="common.welcome" /></p>
                    <div className="w-80 h-80 overflow-hidden">
                        <Lottie className="w-full h-full"
                            animationData={animationData} />
                    </div>
                </div>
                <div className="text-gray-500 text-center text-sm font-medium">
                    © 2024 OpenAuthing
                </div>
            </div>
            <div className="grow px-5 bg-white dark:bg-gray-600">
                <div className="flex flex-col mx-auto w-[480px] h-full min-h-100vh justify-center">
                    <Outlet />
                </div>
            </div>
        </div>
    )
}