import { Button } from "@/components/ui/button"
import { AlertTriangleIcon, CopyIcon, DownloadIcon } from "lucide-react"
import { FormattedMessage, Link, useIntl, useRequest } from "umi"
import AuthService from '@/services/auth.service'
import { useCallback, useState } from "react"
import Spin from "@/components/Spin"
import toast from "react-hot-toast"

export default () => {
    const intl = useIntl()
    const [downloading, setDownloading] = useState<boolean>(false)
    const { data: recoveryCodes, loading } = useRequest(AuthService.getRecoveryCodes)

    const handleDownload = useCallback(() => {
        setDownloading(true)
        try {
            if (recoveryCodes) {
                const content = recoveryCodes.join('\n')
                const element = document.createElement("a");
                const file = new Blob([content], { type: 'text/plain' });
                element.href = URL.createObjectURL(file);
                element.download = "openauthing-recovery-codes.txt";
                document.body.appendChild(element); // Required for this to work in FireFox
                element.click();

                document.body.removeChild(element);
            }
        } finally {
            setDownloading(false)
        }
    }, [recoveryCodes])

    const handleCopy = useCallback(async () => {
        if (recoveryCodes) {

            const content = recoveryCodes.join('\n')
            await navigator.clipboard.writeText(content);
            toast.success(intl.formatMessage({ id: "settings.2fa.recoverycodes.copied.message" }), { duration: 1000 })
        }
    }, [recoveryCodes])


    return (
        <div className="w-full min-w-[680px] max-w-3xl mx-auto flex flex-col justify-center items-center">
            <h1 className="inline-block font-bold text-2xl mt-8 mb-4">
                <FormattedMessage id="settings.2fa.recoverycodes.title" />
            </h1>
            <div className="mx-auto mt-2 rounded-lg shadow-sm border w-full p-8 pb-0">
                <div className="">
                    <h2 className="text-lg font-semibold mb-3">
                        <FormattedMessage id="settings.2fa.recoverycodes.secondary" />
                    </h2>
                    <p className="text-sm text-gray-500 leading-loose mb-4">
                        <FormattedMessage id="settings.2fa.recoverycodes.secondary.desc" />
                    </p>

                    <div className="border rounded mb-4 py-3 flex border-blue-300 bg-blue-50">
                        <div className="flex-none w-[60px] flex justify-end pr-4">
                            <AlertTriangleIcon className="w-6 h-6 stroke-blue-400" />
                        </div>
                        <div className="grow text-xs">
                            <p className="text-black leading-relaxed font-semibold">
                                <FormattedMessage id="settings.2fa.recoverycodes.tips.main" />
                            </p>
                            <p className="text-gray-600 leading-relaxed">
                                <FormattedMessage id="settings.2fa.recoverycodes.tips.secondary" />
                            </p>
                        </div>
                    </div>

                    <Spin spinning={loading}>
                        <div className="bg-secondary/40 px-20 py-8 w-full rounded flex flex-wrap gap-y-3">
                            {recoveryCodes?.map((item, index) => (
                                <div key={index} className="font-semibold basis-1/2">{item}</div>
                            ))}
                        </div>
                    </Spin>


                </div>
                <div className="py-8 flex justify-end gap-x-4">
                    <Button size="xs" className="bg-secondary/40" variant="secondary"
                        onClick={handleDownload}
                        disabled={loading || downloading}>
                        <DownloadIcon className="w-4 h-4" />
                        <FormattedMessage id="common.download" />
                    </Button>
                    <Button size="xs" className="bg-secondary/40" variant="secondary"
                        disabled={loading}
                        onClick={handleCopy}>
                        <CopyIcon className="w-4 h-4" />
                        <FormattedMessage id="common.copy" />
                    </Button>
                </div>
            </div>
            <div className="mt-8 w-full">
                <div className="space-y-3.5">
                    <h2 className="text-sm font-medium text-gray-800">
                        <FormattedMessage id="settings.2fa.recoverycodes.generatenew" />
                    </h2>
                    <p className="text-xs text-gray-600">
                        <FormattedMessage id="settings.2fa.recoverycodes.generatenew.tips" />
                    </p>
                    <div>
                        <Button size="xs" className="bg-secondary/40" variant="secondary"
                            disabled={loading}>
                            <FormattedMessage id="settings.2fa.recoverycodes.generatenew" />
                        </Button>
                    </div>
                </div>
                <div className="mt-6 py-4 border-t">
                    <Link to="/settings/security" className="text-sm font-medium text-primary">
                        <FormattedMessage id="settings.2fa.recoverycodes.gobacktosettings" />
                    </Link>
                </div>
            </div>
        </div>
    )
}