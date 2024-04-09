import { LoaderIcon } from "react-hot-toast"
import { FormattedMessage } from "umi"

type Props = {
    loadingText?: string
}

export default ({
    loadingText
}: Props) => {

    return (
        <div className="absolute top-0 left-0 w-full h-full backdrop-blur-sm bg-white/70 dark:bg-slate-800/60 dark:backdrop-blur-md flex justify-center flex-col">
            <div className="flex h-20 items-center justify-center gap-x-2">
                <LoaderIcon className="w-6 h-6 animate-spin" />
                <span className="text-sm font-medium dark:text-gray-300">
                    {loadingText ?? <FormattedMessage id="common.loading.data" />}
                </span>
            </div>
        </div>
    )
}