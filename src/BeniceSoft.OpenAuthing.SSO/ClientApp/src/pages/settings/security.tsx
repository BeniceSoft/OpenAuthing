import ContentBlock from '@/components/ContentBlock';
import Spin from '@/components/Spin';
import { Button } from '@/components/ui/button';
import { Input, InputLabel } from '@/components/ui/input';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { TwoFactorModelState } from '@/models/twofactor';
import { Key, Smartphone } from 'lucide-react';
import React, { Fragment, useEffect } from 'react'
import { useForm } from 'react-hook-form';
import { Link, connect, useDispatch } from 'umi';

const TwoFactorContentBlockTitle = (props: { is2FaEnabled: boolean, buttonDisabled: boolean }) => {
    const { is2FaEnabled, buttonDisabled } = props
    const dispatch = useDispatch()

    const onDisable = () => {
        dispatch({ type: 'twofactor/disableTwoFactor' })
    }

    return (
        <div className="flex justify-between items-center">
            <div>2FA 身份验证</div>
            {is2FaEnabled &&
                <Popover>
                    <PopoverTrigger asChild={true}>
                        <button className="text-xs py-1.5 px-3 rounded bg-red-500 hover:bg-red-600 transition duration-300 text-white  focus:outline-none">
                            禁用
                        </button>
                    </PopoverTrigger>
                    <PopoverContent
                        className="absolute z-10 w-[320px] shadow-md border right-0 mt-1 p-4 bg-white rounded">
                        <div className="flex flex-col">
                            <p className="text-sm font-normal">禁用 2FA 将会使您的账号安全性降低，请确认操作。</p>
                            <button type="button"
                                className="text-sm text-red-500 aria-disabled:text-red-300 font-normal self-end"
                                disabled={buttonDisabled}
                                aria-disabled={buttonDisabled}
                                onClick={onDisable}>
                                确定禁用
                            </button>
                        </div>
                    </PopoverContent>
                </Popover>
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
                尚未启用 2FA 身份验证
            </p>
            <p className="text-center text-gray-400 text-sm leading-normal">
                2FA 身份验证要求的不仅仅是密码才能登录，从而为您的帐户增加了一层额外的安全保护。
            </p>
            <div className="mt-8 text-center">
                <Link to="/settings/2fa/enable-authenticator"
                    className="bg-blue-600 hover:bg-blue-800 transition duration-300 text-white p-2 px-4 rounded text-sm">
                    启用 2FA 身份验证
                </Link>
            </div>
        </div>
    )
}

const TwoFactorAuthenticationEnabledContent = (props: {
    hasAuthenticator: boolean,
    recoveryCodesLeft: number
}) => {
    const { hasAuthenticator, recoveryCodesLeft } = props
    return (
        <div className="flex flex-col gap-y-4">
            <p className="text-gray-500 text-sm leading-normal">
                2FA 身份验证要求的不仅仅是密码登录，从而为您的帐户增加了一层额外的安全保护。
                2FA 身份验证<span className="text-green-700 font-semibold">已启用</span>
            </p>
            <div className="grid gap-y-3">
                <div>
                    <h2 className="mb-2 text-base font-semibold">2FA 验证方式</h2>
                    <div>
                        <div className="flex border p-3 rounded items-center">
                            <div className="flex-none w-[40px] pt-0.5 self-start">
                                <Smartphone className="w-6 h-6 stroke-gray-600" />
                            </div>
                            <div className="grow text-sm">
                                <div className="mb-2 flex gap-x-2 items-center">
                                    <h3 className="font-semibold inline-block">Authenticator 应用</h3>
                                    {hasAuthenticator && <span className="text-xs border rounded-full px-2 py-[1px] border-green-700 text-green-700">已启用</span>}
                                </div>
                                <div className="text-gray-500 leading-normal text">
                                    使用 Authenticator 应用获取 2FA 身份验证代码，例如：
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
                                    编辑
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                    <h2 className="mb-2 text-base font-semibold">恢复选项</h2>
                    <div>
                        <div className="flex border p-3 rounded items-center">
                            <div className="flex-none w-[40px] pt-0.5 self-start">
                                <Key className="w-6 h-6 stroke-gray-600" />
                            </div>
                            <div className="grow text-sm">
                                <div className="mb-2 flex gap-x-2 items-center">
                                    <h3 className="font-semibold inline-block">恢复码</h3>
                                    <span className="text-xs border rounded-full px-2 py-[1px] border-green-700 text-green-700">
                                        可用 {recoveryCodesLeft} 个
                                    </span>
                                </div>
                                <div className="text-gray-500 leading-normal">
                                    如果您无法访问设备并且无法接收 2FA 身份验证代码，则可以使用恢复码验证您的帐户。
                                </div>
                            </div>
                            <div className="flex-none px-2 align-middle">
                                <Link to="/settings/2fa/recovery-codes"
                                    className="bg-gray-50 border-gray-200 border px-4 py-1.5 rounded text-xs text-gray-700 hover:text-black hover:border-gray-400 transition duration-300">
                                    查看
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
    const dispatch = useDispatch()
    const { register, handleSubmit, formState: { errors, isValid } } = useForm()
    const { isLoading, is2FaEnabled, disableTwoFactorProcessing } = props

    useEffect(() => {
        dispatch({
            type: 'twofactor/fetchTwoFactorState'
        })
    }, [])

    const onChangePasswordSubmit = (data: any) => { }

    return (
        <div className="grid gap-y-6">
            <ContentBlock title="登录密码">
                <form onSubmit={handleSubmit(onChangePasswordSubmit)}>
                    <div className="grid gap-y-4 max-w-md">
                        <InputLabel text="当前密码" required={true}>
                            <Input variant="solid" sizeVariant="xs" type="password"
                                autoComplete="disabled"
                                placeholder="输入当前的登录密码"
                                {...register('oldPassword', { required: true, minLength: 6 })} />
                        </InputLabel>
                        <InputLabel text="新密码" required={true}>
                            <Input variant="solid" sizeVariant="xs" type="password"
                                autoComplete="disabled"
                                placeholder="输入新的密码"
                                {...register('password', { required: true, minLength: 6 })} />
                        </InputLabel>
                        <InputLabel text="确认密码" required={true}>
                            <Input variant="solid" sizeVariant="xs" type="password"
                                autoComplete="disabled"
                                placeholder="重复输入新的密码"
                                {...register('confirmationPassword', { required: true, minLength: 6 })} />
                        </InputLabel>
                        <div>
                            <p className="text-xs mb-2 text-gray-500">密码<span className="text-red-600 font-semibold">至少6位</span>并且<span className="text-red-600 font-semibold">包含数字、字母和符号</span>。</p>
                            <Button variant="secondary"
                                type="submit"
                                disabled={!isValid}>
                                更新密码
                            </Button>
                        </div>
                    </div>
                </form>
            </ContentBlock>
            <ContentBlock title={<TwoFactorContentBlockTitle is2FaEnabled={is2FaEnabled} buttonDisabled={disableTwoFactorProcessing} />}>
                <div className="py-4">
                    <Spin spinning={isLoading}>
                        {is2FaEnabled ?
                            <TwoFactorAuthenticationEnabledContent {...props} /> :
                            <TowFactorAuthenticationNotEnabledContent />}
                    </Spin>
                </div>
            </ContentBlock>
        </div>
    )
}

export default connect(({ loading, twofactor }: { loading: any, twofactor: TwoFactorModelState }) => ({
    isLoading: loading.effects['twofactor/fetchTwoFactorState'],
    disableTwoFactorProcessing: loading.effects['twofactor/disableTwoFactor'],
    ...twofactor
}))(SecurityPage);