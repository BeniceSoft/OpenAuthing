import { LoginWithRecoveryCode } from "@/@types/auth";
import { getSearchParam } from "@/lib/misc";
import { cn } from "@/lib/utils";
import React, { ReactElement, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import OtpInput from "react-otp-input";
import { FormattedHTMLMessage, FormattedMessage, Link, connect, useDispatch, useModel, useSearchParams } from "umi";

type RecoveryCodeInputProps = {
    value?: string
    onChange?: (value?: string) => void
    invalid?: boolean
}
const RecoveryCodeInput = ({ value, onChange, invalid = false }: RecoveryCodeInputProps) => {
    const handleChange = (otp: string) => {
        const code = otp.trim().toUpperCase()

        onChange && onChange(code)
    }

    return (
        <OtpInput onChange={handleChange}
            inputType="text"
            numInputs={10}
            shouldAutoFocus={true}
            containerStyle={"flex gap-x-2"}
            value={value}
            renderSeparator={index => index == 4 && <span className="block mx-1 text-center size-12 leading-[48px]">-</span>}
            renderInput={(props, index) => {
                const inputProps = {
                    ...props,
                    style: undefined,
                    className: cn(
                        "block size-12 text-center border-gray-200 rounded-md text-base font-medium placeholder:text-gray-300 focus:border-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600",
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


const LoginWithRecoveryCodePage: React.FC = function () {
    const [searchParams] = useSearchParams()
    const returnUrl = searchParams.get('returnUrl')
    const { control, register, formState: { isValid, isSubmitting }, handleSubmit } = useForm<LoginWithRecoveryCode>()

    const onSubmit = async (value: LoginWithRecoveryCode) => {
        console.log(value)
    }

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.loginwithrecoverycode.title.text" />
            </h1>
            <p className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <FormattedMessage id="account.loginwithrecoverycode.desc.text" />
            </p>
            <form className="my-5" onSubmit={handleSubmit(onSubmit)}>
                <input type="hidden" {...register("returnUrl")} />
                <div className="space-y-5">
                    <div className="py-2 dark:bg-slate-900">
                        <Controller control={control}
                            name="recoveryCode"
                            render={({ field: { onChange, value } }) => {

                                return (
                                    <RecoveryCodeInput value={value} onChange={onChange} invalid={value === undefined ? false : value.length !== 10} />
                                )
                            }}
                            rules={{
                                required: true,
                                maxLength: 10,
                                minLength: 10
                            }} />
                    </div>
                    <div className="pt-0">
                        <button type="submit"
                            className="p-3 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
                            aria-disabled={isSubmitting || !isValid}
                            disabled={isSubmitting || !isValid}>
                            <FormattedMessage id="account.loginwithrecoverycode.button.verify.text" />
                        </button>
                    </div>
                    <div className="text-sm text-gray-500 flex items-center gap-x-1">

                    </div>
                </div>
            </form >
        </div >
    )
}


export default LoginWithRecoveryCodePage;