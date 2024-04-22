import { ResetPasswordReq } from "@/@types/user"
import PasswordInput from "@/components/password-input"
import { Input, InputLabel } from "@/components/ui/input"
import { getSearchParam } from "@/lib/misc"
import AccountService from "@/services/account.service"
import { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { FormattedMessage, useIntl, useRequest, useSearchParams } from "umi"

export default () => {
    const intl = useIntl()
    const [searchparams] = useSearchParams()
    const [params, setParams] = useState<{ uid: string | null, code: string | null }>()
    const { handleSubmit, register, watch, formState: { errors, isSubmitting, isValid }, control } = useForm<ResetPasswordReq>({ mode: 'onChange' })
    const { run: resetPassword } = useRequest(AccountService.resetPassword, {
        manual: true, onSuccess: (data) => {
        }
    })

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
                    <InputLabel text={intl.formatMessage({ id: 'account.resetpassword.input.password.label' })}>
                        <PasswordInput id="password" name="password" placeholder="********" register={register} invalid={!!errors?.password} />
                    </InputLabel>
                    <InputLabel text={intl.formatMessage({ id: 'account.resetpassword.input.confirmpassword.label' })}>
                        <Input type="password"
                            id="confirmpassword"
                            placeholder="********"
                            autoComplete="new-password"
                            aria-invalid={!!errors?.confirmPassword}
                            {...register('confirmPassword', {
                                required: "Confirm password is required", validate: (value) => {
                                    if (watch('password') !== value) {
                                        return "Your passwords do no match";
                                    }
                                }
                            })} />
                    </InputLabel>
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