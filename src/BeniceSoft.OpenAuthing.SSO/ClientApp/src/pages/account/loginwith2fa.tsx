import React, { useState } from "react";
import OtpInput from 'react-otp-input';
import { Link, useSearchParams, connect, useDispatch } from 'umi'
import { getSearchParam } from "@/lib/misc";
import { LoginModelState } from "@/models/login";

interface LoginWith2FaProps {
    isLoggingIn: boolean
}

const LoginWith2Fa: React.FC<LoginWith2FaProps> = function (props: LoginWith2FaProps) {
    const [searchParams] = useSearchParams()
    const [otp, setOtp] = useState('');
    const dispatch = useDispatch()
    const { isLoggingIn } = props

    const onSubmit = () => {
        dispatch({
            type: 'login/loginWith2Fa',
            payload: {
                twoFactorCode: otp,
                returnUrl: getSearchParam(searchParams, 'returnUrl') ?? '',
                rememberMe: true,
                rememberMachine: false
            }
        })
    }
    return (
        <div className="rounded-lg shadow-[0_8px_24px_0px_rgba(45,46,50,.15)] lg:w-[500px] md:w-[480px] h-[400px] overflow-hidden bg-white">
            <div className="w-full h-full px-16">
                <div className="text-center p-10 pt-14">
                    <h1 className="text-3xl">2FA 验证</h1>
                </div>
                <OtpInput value={otp}
                    onChange={otp => setOtp(otp.toUpperCase())}
                    numInputs={6}
                    shouldAutoFocus={true}
                    containerStyle="flex justify-between space-x-3 mb-4"
                    inputStyle="mt-2 w-1/6 transition block rounded-md border-gray-300 focus:border-blue-500 placeholder-slate-400 text-md"
                    renderInput={(props: any) => (<input {...props} />)} />
                <button type="button"
                    className="rounded-md mt-6 bg-blue-500 hover:bg-blue-600 aria-disabled:bg-blue-300 w-full h-10 text-white transition aria-disabled:cursor-not-allowed"
                    disabled={isLoggingIn || otp.length < 6}
                    aria-disabled={isLoggingIn || otp.length < 6}
                    onClick={onSubmit}>
                    验证
                </button>
                <p className="mt-6 text-sm text-gray-400 leading-relaxed">
                    Don&apos;t have access to your authenticator device? You can&nbsp;
                    <Link to={{ pathname: '/account/loginwithrecoverycode', search: `?returnUrl=${searchParams.get('returnUrl')}` }} className="text-blue-500">log in with a recovery code</Link>
                </p>
            </div>
        </div>
    )
}

export default connect(({ loading, login }: { loading: any, login: LoginModelState }) => ({
    ...login,
    isLoggingIn: loading.effects['login/loginWith2Fa']
}))(LoginWith2Fa)