import { ResetPasswordReq, ResetPasswordValidationMethod } from "@/@types/user"
import { getSearchParam } from "@/lib/misc"
import AccountService from "@/services/account.service"
import { CheckIcon, XIcon } from "lucide-react"
import { HSStrongPassword } from "preline/preline"
import { useEffect, useRef, useState } from "react"
import { useForm } from "react-hook-form"
import { FormattedMessage, Link, history, useRequest, useSearchParams } from "umi"

export default () => {
    const [searchparams] = useSearchParams()
    const [params, setParams] = useState<{ uid: string | null, code: string | null }>()
    const [validationMethod, setValidationMethod] = useState<ResetPasswordValidationMethod>('email')
    const { handleSubmit, register, formState: { isSubmitting, isValid }, setValue } = useForm<ResetPasswordReq>()
    const { run: resetPassword } = useRequest(AccountService.resetPassword, {
        manual: true, onSuccess: (data) => {
        }
    })

    useEffect(() => {
        HSStrongPassword.autoInit()
    }, [])

    useEffect(() => {
        const uid = getSearchParam(searchparams, 'uid')
        const code = getSearchParam(searchparams, 'code')

        setParams({ uid, code })
    }, [searchparams])

    const onSubmit = async (value: any) => {
        const input = {
            ...value,
            ...params
        }
        await resetPassword(input)
    }

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.resetpassword.title.text" />
            </h1>
            <p className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <FormattedMessage id="account.resetpassword.desc.text" />
            </p>
            <form className="my-5" onSubmit={handleSubmit(onSubmit)}>
                <div className="space-y-5">
                    <div className="relative">
                        <label htmlFor="password" className="block text-gray-700 text-sm mb-2 font-medium dark:text-white">
                            <FormattedMessage id="account.resetpassword.input.password.label" />
                        </label>
                        <input type="password"
                            id="password"
                            className="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600"
                            placeholder="********"
                            {...register('password', { required: true, minLength: 6 })} />
                        <div id="new-password-strong-popover"
                            className="hidden absolute z-10 w-full bg-white shadow-xl rounded-lg p-4 dark:bg-gray-800 dark:border dark:border-gray-700 dark:divide-gray-700">
                            <div id="hs-strong-password-in-popover" data-hs-strong-password='{
                                "target": "#password",
                                "hints": "#new-password-strong-popover",
                                "stripClasses": "hs-strong-password:opacity-100 hs-strong-password-accepted:bg-teal-500 h-2 flex-auto rounded-full bg-blue-500 opacity-50 mx-1",
                                "mode": "popover"
                            }'
                                className="flex mt-2 -mx-1">
                            </div>

                            <h4 className="mt-3 text-sm font-semibold text-gray-800 dark:text-white">
                                <FormattedMessage id="common.password.rule" />
                            </h4>

                            <ul className="space-y-1 text-sm text-gray-500">
                                <li data-hs-strong-password-hints-rule-text="min-length" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                    <span className="hidden" data-check="">
                                        <CheckIcon className="w-4 h-4" />
                                    </span>
                                    <span data-uncheck="">
                                        <XIcon className="w-4 h-4" />
                                    </span>
                                    <FormattedMessage id="common.password.rule.length" />
                                </li>
                                <li data-hs-strong-password-hints-rule-text="lowercase" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                    <span className="hidden" data-check="">
                                        <CheckIcon className="w-4 h-4" />
                                    </span>
                                    <span data-uncheck="">
                                        <XIcon className="w-4 h-4" />
                                    </span>
                                    <FormattedMessage id="common.password.rule.lowercase" />
                                </li>
                                <li data-hs-strong-password-hints-rule-text="uppercase" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                    <span className="hidden" data-check="">
                                        <CheckIcon className="w-4 h-4" />
                                    </span>
                                    <span data-uncheck="">
                                        <XIcon className="w-4 h-4" />
                                    </span>
                                    <FormattedMessage id="common.password.rule.uppercase" />
                                </li>
                                <li data-hs-strong-password-hints-rule-text="numbers" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                    <span className="hidden" data-check="">
                                        <CheckIcon className="w-4 h-4" />
                                    </span>
                                    <span data-uncheck="">
                                        <XIcon className="w-4 h-4" />
                                    </span>
                                    <FormattedMessage id="common.password.rule.numbers" />
                                </li>
                                <li data-hs-strong-password-hints-rule-text="special-characters" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                    <span className="hidden" data-check="">
                                        <CheckIcon className="w-4 h-4" />
                                    </span>
                                    <span data-uncheck="">
                                        <XIcon className="w-4 h-4" />
                                    </span>
                                    <FormattedMessage id="common.password.rule.specialchar" />
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div>
                        <label htmlFor="password" className="block text-gray-700 text-sm mb-2 font-medium dark:text-white">
                            <FormattedMessage id="account.resetpassword.input.confirmpassword.label" />
                        </label>
                        <input type="password"
                            className="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600"
                            placeholder="********"
                            {...register('confirmPassword', { required: true, minLength: 6 })} />
                    </div>
                    <div>
                        <button type="submit"
                            className="p-3 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
                            aria-disabled={isSubmitting || !isValid}
                            disabled={isSubmitting || !isValid}>
                            <FormattedMessage id="account.resetpassword.button.resetpassword.text" />
                        </button>
                    </div>
                </div>
            </form >
        </div >
    )
}