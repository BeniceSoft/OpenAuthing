import { Link, Navigate, connect, history, useLocation } from "umi"

const ShowRecoveryCodesPage = () => {
    const { state } = useLocation()
    const { recoveryCodes } = (state ?? {}) as { recoveryCodes: string[] | undefined }

    if (!recoveryCodes) {
        return (
            <Navigate to="/settings/security" />
        )
    }

    const onComplete = () => {
        history.push({
            pathname: '/settings/security'
        })
    }

    return (
        <div className="w-full max-w-[800px] mx-auto flex flex-col justify-center items-center">
            <h1 className="inline-block font-bold text-2xl mt-4 mb-2">保存恢复码</h1>
            <div className="mx-auto mt-2 rounded-md shadow border w-full p-8 pb-0">
                <div className="pb-8 border-b">
                    <h2 className="text-lg font-semibold mb-3">导出恢复码文件</h2>
                    <p className="text-sm text-gray-500 leading-loose mb-4">
                        导出恢复码文件到本地，后续可以使用恢复代码作为第二个因素来进行身份验证，以防您无法访问您的设备。
                    </p>

                    <div className="border rounded mb-4 py-3 flex border-yellow-400 bg-yellow-50">
                        <div className="flex-none w-[60px] flex justify-end pr-4">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 stroke-yellow-600">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z" />
                            </svg>
                        </div>
                        <div className="grow text-sm">
                            <p className="text-black leading-6 font-semibold">请谨慎保存恢复码文件</p>
                            <p className="text-gray-600 leading-6">如果您丢失了设备并且找不到您的恢复代码，您将无法访问您的帐户。</p>
                        </div>
                    </div>

                    <div className="bg-gray-50 px-20 py-8 w-full rounded flex flex-wrap gap-y-3">
                        {recoveryCodes.map((item, index) => (
                            <div className="font-semibold basis-1/2">{item}</div>
                        ))}
                    </div>

                    <div className="flex justify-end mt-8">
                        <button className="text-sm bg-blue-600 text-white px-6 py-1.5 rounded flex items-center gap-x-1">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M3 16.5v2.25A2.25 2.25 0 005.25 21h13.5A2.25 2.25 0 0021 18.75V16.5M16.5 12L12 16.5m0 0L7.5 12m4.5 4.5V3" />
                            </svg>
                            导出恢复码
                        </button>
                    </div>
                </div>
                <div className="py-8 flex justify-end gap-x-2">
                    <Link to="/settings/security"
                        className="rounded px-6 py-1 text-sm hover:bg-gray-200 text-blue-600 transition duration-300">
                        取消
                    </Link>
                    <button type="button"
                        className="rounded px-6 py-1 text-sm bg-blue-600 text-white transition duration-300 hover:bg-blue-700 aria-disabled:bg-blue-300 aria-disabled:cursor-not-allowed"
                        aria-disabled={false}
                        disabled={false}
                        onClick={onComplete}>
                        完成
                    </button>
                </div>
            </div>
        </div>
    )
}

export default connect(() => ({

}))(ShowRecoveryCodesPage)