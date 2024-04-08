import { ChangePasswordModel } from '@/@types/settings';
import ContentBlock from '@/components/ContentBlock';
import Spin from '@/components/Spin';
import { Button } from '@/components/ui/button';
import { Input, InputLabel } from '@/components/ui/input';
import { CheckIcon, Key, Smartphone, XIcon } from 'lucide-react';
import React from 'react'
import { useForm } from 'react-hook-form';
import { FormattedMessage, Link, useIntl, useRequest } from 'umi';
import AuthService from '@/services/auth.service'

type ChangePasswordFormProps = {
    onSubmit: (data: ChangePasswordModel) => Promise<void>
}

const ChangePasswordForm = ({
    onSubmit
}: ChangePasswordFormProps) => {
    const { register, handleSubmit, formState: { errors, isValid } } = useForm<ChangePasswordModel>()
    const intl = useIntl()

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <div className="flex flex-col gap-y-4 lg:max-w-md">
                <InputLabel text={intl.formatMessage({ id: "settings.secrity.password.current" })} required={true}>
                    <Input type="password"
                        autoComplete="disabled"
                        {...register('oldPassword', { required: true, minLength: 6 })} />
                </InputLabel>
                <InputLabel text={intl.formatMessage({ id: "settings.secrity.password.new" })} required={true}
                    className="relative">
                    <Input type="password"
                        id="new-password"
                        autoComplete="disabled"
                        {...register('newPassword', { required: true, minLength: 6 })} />
                    <div id="new-password-strong-popover"
                        className="hidden absolute z-10 w-full bg-white shadow-xl rounded-lg p-4 dark:bg-gray-800 dark:border dark:border-gray-700 dark:divide-gray-700">
                        <div id="hs-strong-password-in-popover" data-hs-strong-password='{
                                "target": "#new-password",
                                "hints": "#new-password-strong-popover",
                                "stripClasses": "hs-strong-password:opacity-100 hs-strong-password-accepted:bg-teal-500 h-2 flex-auto rounded-full bg-blue-500 opacity-50 mx-1",
                                "mode": "popover"
                            }'
                            className="flex mt-2 -mx-1">
                        </div>

                        <h4 className="mt-3 text-sm font-semibold text-gray-800 dark:text-white">
                            <FormattedMessage id="settings.secrity.password.rule" />
                        </h4>

                        <ul className="space-y-1 text-sm text-gray-500">
                            <li data-hs-strong-password-hints-rule-text="min-length" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                <span className="hidden" data-check="">
                                    <CheckIcon className="w-4 h-4" />
                                </span>
                                <span data-uncheck="">
                                    <XIcon className="w-4 h-4" />
                                </span>
                                <FormattedMessage id="settings.secrity.password.rule.length" />
                            </li>
                            <li data-hs-strong-password-hints-rule-text="lowercase" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                <span className="hidden" data-check="">
                                    <CheckIcon className="w-4 h-4" />
                                </span>
                                <span data-uncheck="">
                                    <XIcon className="w-4 h-4" />
                                </span>
                                <FormattedMessage id="settings.secrity.password.rule.lowercase" />
                            </li>
                            <li data-hs-strong-password-hints-rule-text="uppercase" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                <span className="hidden" data-check="">
                                    <CheckIcon className="w-4 h-4" />
                                </span>
                                <span data-uncheck="">
                                    <XIcon className="w-4 h-4" />
                                </span>
                                <FormattedMessage id="settings.secrity.password.rule.uppercase" />
                            </li>
                            <li data-hs-strong-password-hints-rule-text="numbers" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                <span className="hidden" data-check="">
                                    <CheckIcon className="w-4 h-4" />
                                </span>
                                <span data-uncheck="">
                                    <XIcon className="w-4 h-4" />
                                </span>
                                <FormattedMessage id="settings.secrity.password.rule.numbers" />
                            </li>
                            <li data-hs-strong-password-hints-rule-text="special-characters" className="hs-strong-password-active:text-teal-500 flex items-center gap-x-2">
                                <span className="hidden" data-check="">
                                    <CheckIcon className="w-4 h-4" />
                                </span>
                                <span data-uncheck="">
                                    <XIcon className="w-4 h-4" />
                                </span>
                                <FormattedMessage id="settings.secrity.password.rule.specialchar" />
                            </li>
                        </ul>
                    </div>
                </InputLabel>
                <InputLabel text={intl.formatMessage({ id: "settings.secrity.password.confirm" })} required={true}
                    errorMessage={errors.confirmationPassword?.message?.toString()}>
                    <Input type="password"
                        autoComplete="disabled"
                        {...register('confirmationPassword', { required: { value: true, message: "Confirmation password is required" } })} />
                </InputLabel>
                <div className="flex items-center justify-between gap-x-3">
                    <Button variant="secondary"
                        type="submit"
                        disabled={!isValid}>
                        <FormattedMessage id="settings.secrity.password.submit" />
                    </Button>
                    <Link to="/"
                        className="text-xs text-primary/80 font-medium">
                        <FormattedMessage id="settings.secrity.password.forgot" />
                    </Link>
                </div>
            </div>
        </form>
    )
}

const TwoFactorContentBlockTitle = (props: { is2FaEnabled: boolean, buttonDisabled: boolean }) => {
    const { is2FaEnabled, buttonDisabled } = props

    const onDisable = () => {

    }

    return (
        <div className="flex justify-between items-center">
            <h2><FormattedMessage id="settings.secrity.2fa.title" /></h2>
            {is2FaEnabled &&
                <button className="text-xs py-1.5 px-3 rounded bg-red-500 hover:bg-red-600 transition duration-300 text-white  focus:outline-none">
                    <FormattedMessage id="common.disable" />
                </button>
            }
        </div>
    )
}

const TowFactorAuthenticationNotEnabledContent = () => {
    return (
        <div className="flex-column px-8">
            <div className="flex justify-center p-4">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 stroke-gray-600">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M16.5 10.5V6.75a4.5 4.5 0 10-9 0v3.75m-.75 11.25h10.5a2.25 2.25 0 002.25-2.25v-6.75a2.25 2.25 0 00-2.25-2.25H6.75a2.25 2.25 0 00-2.25 2.25v6.75a2.25 2.25 0 002.25 2.25z" />
                </svg>
            </div>
            <p className="text-center text-xl font-semibold mb-4">
                <FormattedMessage id="settings.secrity.2fa.notenabled" />
            </p>
            <p className="text-center text-gray-400 text-sm leading-normal">
                <FormattedMessage id="settings.secrity.2fa.desc" />
            </p>
            <div className="mt-8 text-center">
                <Link to="/settings/2fa/enable-authenticator"
                    className="bg-blue-600 hover:bg-blue-800 transition duration-300 text-white p-2 px-4 rounded text-sm">
                    <FormattedMessage id="settings.secrity.2fa.enable" />
                </Link>
            </div>
        </div>
    )
}

const TwoFactorAuthenticationEnabledContent = (props: {
    hasAuthenticator: boolean,
    recoveryCodesLeft: number
}) => {
    const { hasAuthenticator, recoveryCodesLeft = 0 } = props
    return (
        <div className="">
            <p className="text-gray-500 text-sm leading-normal -mt-4 mb-6">
                <FormattedMessage id="settings.secrity.2fa.desc" />
            </p>
            <div className="grid gap-y-6">
                <div>
                    <h2 className="mb-2 text-base text-gray-800 font-semibold"><FormattedMessage id="settings.secrity.2fa.methods.title" /></h2>
                    <div>
                        <div className="flex border p-4 rounded-lg items-center">
                            <div className="flex-none w-[40px] pt-0.5 self-start">
                                <Smartphone className="w-6 h-6 stroke-gray-600" />
                            </div>
                            <div className="grow text-sm">
                                <div className="mb-2 flex gap-x-2 items-center">
                                    <h3 className="font-semibold text-gray-700 inline-block"><FormattedMessage id="settings.secrity.2fa.methods.authenticatorapp" /></h3>
                                    {hasAuthenticator && <span className="text-xs border rounded-full px-2 py-[1px] border-green-700 text-green-700">已启用</span>}
                                </div>
                                <div className="text-gray-500 leading-normal text">
                                    <FormattedMessage id="settings.secrity.2fa.methods.authenticatorapp.desc" />
                                    <a href="https://www.microsoft.com/en-us/security/mobile-authenticator-app"
                                        className="text-blue-500 hover:text-blue-600"
                                        target="_blank">
                                        Microsoft Authenticator
                                    </a>、
                                    <a href="https://googleauthenticator.net/"
                                        className="text-blue-500 hover:text-blue-600"
                                        target="_blank">
                                        Google Authenticator
                                    </a>
                                </div>
                            </div>
                            <div className="flex-none px-2 align-middle">
                                <button type="button"
                                    className="bg-gray-50 border-gray-200 border px-4 py-1.5 rounded text-xs text-gray-700 hover:text-black hover:border-gray-400 transition duration-300">
                                    <FormattedMessage id="common.edit" />
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <h2 className="mb-2 text-base text-gray-800 font-semibold"><FormattedMessage id="settings.secrity.2fa.recoveryoptions" /></h2>
                    <div>
                        <div className="flex border p-4 rounded-lg items-center">
                            <div className="flex-none w-[40px] pt-0.5 self-start">
                                <Key className="w-6 h-6 stroke-gray-600" />
                            </div>
                            <div className="grow text-sm">
                                <div className="mb-2 flex gap-x-2 items-center">
                                    <h3 className="font-semibold text-gray-800 inline-block"><FormattedMessage id="settings.secrity.2fa.recoveryoptions.recoverycode" /></h3>
                                    <span className="text-xs border rounded-full px-2 py-[1px] border-green-700 text-green-700">
                                        可用 {recoveryCodesLeft} 个
                                    </span>
                                </div>
                                <div className="text-gray-500 leading-normal">
                                    <FormattedMessage id="settings.secrity.2fa.recoveryoptions.recoverycode.desc" />
                                </div>
                            </div>
                            <div className="flex-none px-2 align-middle">
                                <Link to="/settings/2fa/recovery-codes"
                                    className="bg-gray-50 border-gray-200 border px-4 py-1.5 rounded text-xs text-gray-700 hover:text-black hover:border-gray-400 transition duration-300">
                                    <FormattedMessage id="common.view" />
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export interface SecurityPageProps {
    isLoading: boolean
    disableTwoFactorProcessing: boolean
    hasAuthenticator: boolean
    is2FaEnabled: boolean
    isMachineRemembered: boolean
    recoveryCodesLeft: number
}

const SecurityPage: React.FC<SecurityPageProps> = (props: SecurityPageProps) => {
    const intl = useIntl()
    const { data: twoFactorState = {}, loading: twoFactorStateLoading } = useRequest(AuthService.getTwoFactorState)

    const { is2FaEnabled } = twoFactorState;

    const handleChangePasswordSubmit = async (data: any) => { }

    return (
        <div className="grid gap-y-8">
            <ContentBlock title={intl.formatMessage({ id: "settings.secrity.password.title" })}>
                <ChangePasswordForm onSubmit={handleChangePasswordSubmit} />
            </ContentBlock>
            <ContentBlock title={<TwoFactorContentBlockTitle is2FaEnabled={is2FaEnabled}
                buttonDisabled={false} />}>
                <div className="py-4">
                    <Spin spinning={twoFactorStateLoading}>
                        {is2FaEnabled ?
                            <TwoFactorAuthenticationEnabledContent {...twoFactorState} /> :
                            <TowFactorAuthenticationNotEnabledContent />}
                    </Spin>
                </div>
            </ContentBlock>
        </div>
    )
}

export default SecurityPage