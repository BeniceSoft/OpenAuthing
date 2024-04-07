import LangSelect from "@/components/LangSelect";
import Logo from "@/components/Logo";
import { FormattedMessage, Link, Outlet } from "umi";

export default function () {
    return (
        <div className="h-screen w-screen p-0 flex">
            <div className="py-6 px-10 hidden lg:flex w-1/4 min-w-[428px] max-w-2xl bg-gray-100 justify-between flex-col dark:bg-neutral-950">
                <div className="flex justify-between items-center">
                    <Link to="/"><Logo className="w-48" /></Link>
                    <LangSelect />
                </div>
                <div className="mx-auto space-y-4">
                    <p className="text-2xl font-medium tex-gray-600"><FormattedMessage id="common.welcome" /></p>
                    <div className="w-80 h-80 overflow-hidden bg-red-50"></div>
                </div>
                <div className="text-gray-500 text-center text-sm font-medium">
                    © 2024 OpenAuthing
                </div>
            </div>
            <div className="grow px-5">
                <div className="flex flex-col mx-auto w-[480px] h-full min-h-100vh justify-center">
                    <Outlet />
                </div>
            </div>
        </div>
    )
}