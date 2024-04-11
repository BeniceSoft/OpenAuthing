import { Button } from "@/components/ui/button"
import { getSearchParam } from "@/lib/misc"
import { FormattedHTMLMessage, FormattedMessage, Link, useSearchParams } from "umi"

export default () => {
    const [searchparams] = useSearchParams()
    const email = getSearchParam(searchparams, "email")

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.emailverification.title.text" />
            </h1>
            <div className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <p><FormattedHTMLMessage id="account.emailverification.desc1.text" values={{ email }} /></p>
                <p><FormattedMessage id="account.emailverification.desc2.text" /></p>
            </div>
            <div className="mt-5 space-y-4 font-medium">
                <Link to="/account/login">
                    <button type="button"
                        className="px-3 py-2 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none">
                        <FormattedMessage id="account.emailverification.btn.continue" />
                    </button>
                </Link>
                <div className="text-sm">
                    <FormattedMessage id="account.emailverification.didtrecieved" />
                    <Button variant="link">
                        <FormattedMessage id="account.emailverification.btn.resend" />
                    </Button>
                </div>
            </div>
        </div>
    )
}