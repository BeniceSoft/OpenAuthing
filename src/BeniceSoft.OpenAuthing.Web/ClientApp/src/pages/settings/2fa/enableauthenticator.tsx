import { QRCodeCanvas } from 'qrcode.react';
import React, { useEffect, useState } from 'react';
import OTPInput from 'react-otp-input';
import { Link, connect, useDispatch } from 'umi';
import { TwoFactorModelState } from '@/models/twofactor';

export interface EnableAuthenticatorPageProps {
    isLoadingAuthenticatorUri: boolean
    isLoggingIn: boolean
    authenticatorUri?: string
}

const EnableAuthenticatorPage: React.FC<EnableAuthenticatorPageProps> = (props: EnableAuthenticatorPageProps) => {
    const { isLoadingAuthenticatorUri, isLoggingIn, authenticatorUri = '' } = props
    const [code, setCode] = useState('')
    const dispatch = useDispatch()

    useEffect(() => {
        dispatch({
            type: 'twofactor/fetchAuthenticatorUri'
        })
    }, [])

    const onRefresh = () => {
        dispatch({
            type: 'twofactor/fetchAuthenticatorUri'
        })
    }

    const onSubmit = () => {
        dispatch({
            type: 'twofactor/enableAuthenticator',
            payload: {
                code
            }
        })
    }


    return (
        <div className="w-full max-w-[800px] mx-auto flex flex-col justify-center items-center">
            <h1 className="inline-block font-bold text-2xl mt-4 mb-2">启用 2FA 身份验证</h1>
            <div className="mx-auto mt-2 rounded-md shadow border w-[860px] p-8 pb-0">
                <div className="pb-8 border-b">
                    <h2 className="text-lg font-semibold mb-3">设置 Authenticator 应用</h2>
                    <p className="text-sm text-gray-500 leading-loose mb-4">
                        可以使用例如：
                        <a href="https://www.microsoft.com/en-us/security/mobile-authenticator-app">Microsoft Authenticator</a>、
                        <a href="https://googleauthenticator.net/">Google Authenticator</a>
                        等密码管理器具有应用程序和浏览器扩展程序，您可以使用它们在登录期间收到提示时获取 2FA 代码。
                    </p>
                    <h3 className="text-base mb-3 font-medium">扫描二维码</h3>
                    <p className="text-sm text-gray-500 leading-loose mb-3">
                        使用 Authenticator 应用进行扫描，然后填写由 Authenticator 应用提供的代码。
                    </p>
                    <div className="border rounded p-2 inline-block mb-3">
                        <div className="h-[180px] w-[180px] flex justify-center items-center">
                            {isLoadingAuthenticatorUri ?
                                <span className="block animate-spin">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6 stroke-gray-300">
                                        <path strokeLinecap="round" strokeLinejoin="round" d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99" />
                                    </svg>
                                </span> :
                                <QRCodeCanvas value={authenticatorUri}
                                    size={180}
                                    className="cursor-pointer"
                                    onClick={onRefresh} />}
                        </div>
                    </div>
                    <label className="block">
                        <OTPInput value={code}
                            onChange={setCode}
                            numInputs={6}
                            containerStyle="text-sm gap-x-1"
                            inputStyle="w-[35px] transition block rounded-md border-gray-300 focus:border-blue-500 placeholder-slate-400 text-xs"
                            renderInput={(props: any) => (<input {...props} />)}
                            renderSeparator={index => index === 2 ? <span className="px-1" /> : null} />
                    </label>
                </div>
                <div className="py-8 flex justify-end gap-x-2">
                    <Link to="/settings/security"
                        className="rounded px-6 py-1 text-sm hover:bg-gray-200 text-blue-600 transition duration-300">
                        取消
                    </Link>
                    <button type="button"
                        className="rounded bg-blue-600 hover:bg-blue-700 text-white px-6 py-1 text-sm transition duration-300 aria-disabled:bg-blue-300 aria-disabled:cursor-not-allowed"
                        disabled={code.length < 6}
                        aria-disabled={code.length < 6}
                        onClick={onSubmit}>
                        确定
                    </button>
                </div>
            </div>
        </div>
    )
}

export default connect(({ loading, twofactor }: { loading: any, twofactor: TwoFactorModelState }) => ({
    isLoadingAuthenticatorUri: loading.effects['twofactor/fetchAuthenticatorUri'],
    isLoggingIn: loading.effects['twofactor/enableAuthenticator'],
    ...twofactor
}))(EnableAuthenticatorPage)