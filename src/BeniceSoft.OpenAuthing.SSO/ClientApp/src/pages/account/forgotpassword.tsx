import { ForgotPasswordReq, ResetPasswordValidationMethod } from "@/@types/user"
import { Input, InputLabel } from "@/components/ui/input"
import useReturnUrl from "@/hooks/useReturnUrl"
import AccountService from "@/services/account.service"
import { useState } from "react"
import { useForm } from "react-hook-form"
import { FormattedMessage, Link, history, useIntl, useRequest } from "umi"

export default () => {
    const intl = useIntl()
    const [validationMethod, setValidationMethod] = useState<ResetPasswordValidationMethod>('email')
    const returnUrl = useReturnUrl()

    const { handleSubmit, register, formState: { errors, isSubmitting, isValid } } = useForm<ForgotPasswordReq>()
    const { run: forgotPassword } = useRequest(AccountService.forgotPassword, {
        manual: true, onSuccess: (data, params) => {
            history.push({
                pathname: "/account/email-verification",
                search: "?email=" + params[0].email
            })
        }
    })

    const onSubmit = async (value: any) => {
        await forgotPassword(value)
    }

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.forgotpassword.title.text" />
            </h1>
            <p className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <FormattedMessage id="account.forgotpassword.desc.text" />
            </p>
            <form className="my-5" onSubmit={handleSubmit(onSubmit)}>
                <div className="space-y-5">
                    <InputLabel text={intl.formatMessage({ id: 'account.forgotpassword.input.emailaddress.label' })}>
                        <Input type="email"
                            id="emailaddress"
                            placeholder="you@sample.com"
                            autoComplete="new-password"
                            aria-invalid={!!errors?.email}
                            {...register('email', {
                                required: 'Email address is required',
                                pattern: {
                                    value: /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/,
                                    message: 'Invalid email address',
                                }
                            })} />
                    </InputLabel>
                    <div>
                        <button type="submit"
                            className="px-3 py-2.5 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
                            aria-disabled={isSubmitting || !isValid}
                            disabled={isSubmitting || !isValid}>
                            <FormattedMessage id="account.forgotpassword.button.verify.text" />
                        </button>
                    </div>
                    <div className="text-sm text-gray-400 flex items-center gap-x-1">
                        <FormattedMessage id="account.forgotpassword.didnotforgot" />
                        <Link to={{ pathname: '/account/login', search: `?returnUrl=${returnUrl}` }}
                            className="text-blue-500">
                            <FormattedMessage id="account.forgotpassword.link.gotosignin" />
                        </Link>
                    </div>
                </div>
            </form >
        </div >
    )
}