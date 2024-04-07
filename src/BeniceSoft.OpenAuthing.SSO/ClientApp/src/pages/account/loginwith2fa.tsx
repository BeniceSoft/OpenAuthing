import React, { useEffect } from "react";
import { Link, useSearchParams, FormattedMessage, FormattedHTMLMessage, useModel } from 'umi'
import { Controller, useForm } from "react-hook-form";
import { LoginWith2FaModel } from "@/@types/auth";
import OtpInput from 'react-otp-input';
import { cn } from "@/lib/utils";

type AuthenticationCodeInputProps = {
    value?: string
    onChange?: (value?: string) => void
    invalid?: boolean
}
const AuthenticationCodeInput = ({ value, onChange, invalid = false }: AuthenticationCodeInputProps) => {
    const handleChange = (otp: string) => {
        const code = otp.trim()

        onChange && onChange(code)
    }

    return (
        <OtpInput onChange={handleChange}
            inputType="number"
            numInputs={6}
            shouldAutoFocus={true}
            containerStyle={"flex gap-x-4"}
            value={value}
            renderInput={(props, index) => {
                const inputProps = {
                    ...props,
                    style: undefined,
                    className: cn(
                        "block size-12 text-center border-gray-200 rounded-md text-base font-medium placeholder:text-gray-300 focus:border-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-700 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600",
                        invalid && index >= (value?.length ?? -1) ? "focus:ring-red-400 focus:border-red-400" : "focus:ring-blue-500"
                    )
                }

                return (
                    <input {...inputProps} placeholder="○" />
                )
            }
            } />
    )
}

const LoginWith2FaPage: React.FC = function () {
    const [searchParams] = useSearchParams()
    const returnUrl = searchParams.get('returnUrl')
    const { control, register, formState: { isValid, isSubmitting }, setValue, handleSubmit } = useForm<LoginWith2FaModel>()

    useEffect(() => {
        returnUrl && setValue('returnUrl', returnUrl)
    }, [returnUrl])

    const { loginWith2Fa } = useModel('account.login', model => ({
        loginWith2Fa: model.loginWith2Fa
    }))

    const onSubmit = async (value: LoginWith2FaModel) => {
        await loginWith2Fa(value)
    }

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.loginwith2fa.title.text" />
            </h1>
            <p className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <FormattedMessage id="account.loginwith2fa.desc.text" />
            </p>
            <form className="my-5" onSubmit={handleSubmit(onSubmit)}>
                <input type="hidden" {...register("returnUrl")} />
                <div className="space-y-5">
                    <div className="py-2">
                        <Controller control={control}
                            name="twoFactorCode"
                            render={({ field: { onChange, value } }) => (
                                <AuthenticationCodeInput value={value} onChange={onChange} invalid={value === undefined ? false : value.length !== 6} />
                            )}
                            rules={{
                                required: true,
                                maxLength: 6,
                                minLength: 6
                            }} />
                    </div>
                    <div className="pt-0">
                        <button type="submit"
                            className="p-3 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
                            aria-disabled={isSubmitting || !isValid}
                            disabled={isSubmitting || !isValid}>
                            <FormattedMessage id="account.loginwith2fa.button.verify.text" />
                        </button>
                    </div>
                    <div className="text-sm text-gray-400 flex items-center gap-x-1">
                        <FormattedMessage id="account.loginwith2fa.havingProblems" />
                        <Link to={{ pathname: '/account/loginwithrecoverycode', search: `?returnUrl=${returnUrl}` }}
                            className="text-blue-500">
                            <FormattedHTMLMessage id="account.loginwith2fa.link.useRecoveryCode.text" />
                        </Link>
                    </div>
                </div>
            </form >
        </div >
    )
}

export default LoginWith2FaPage