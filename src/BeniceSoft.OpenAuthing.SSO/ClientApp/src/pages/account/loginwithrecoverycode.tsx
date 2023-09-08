import { getSearchParam } from "@/lib/misc";
import React, { ReactElement, useState } from "react";
import OtpInput from "react-otp-input";
import { connect, useDispatch, useSearchParams } from "umi";

export interface LoginWithRecoveryCodePageProps {
    isLoading: boolean
}

const LoginWithRecoveryCodePage: React.FC<LoginWithRecoveryCodePageProps> = function (props: LoginWithRecoveryCodePageProps) {
    const [searchParams] = useSearchParams()
    const dispatch = useDispatch()
    const [code, setCode] = useState('')
    const { isLoading } = props

    const onSubmit = () => {
        const recoveryCode = code.slice(0, 5) + '-' + code.slice(5);
        dispatch({
            type: 'login/loginWithRecoveryCode',
            payload: {
                recoveryCode: recoveryCode,
                returnUrl: getSearchParam(searchParams, 'returnUrl') ?? '',
            }
        })
    }

    return (
        <div className="rounded-lg shadow-[0_8px_24px_0px_rgba(45,46,50,.15)] lg:w-[640px] w-[560px] h-[400px] overflow-hidden bg-white">
            <div className="w-full h-full px-16">
                <div className="text-center p-10 pt-14">
                    <h1 className="text-3xl">Recovery Code</h1>
                </div>
                <OtpInput value={code}
                    onChange={otp => setCode(otp.toUpperCase())}
                    numInputs={10}
                    renderInput={props => (<input {...props} />)}
                    renderSeparator={index => index == 4 && <span className="text-center w-[5%]">-</span>}
                    shouldAutoFocus={true}
                    containerStyle="flex justify-between space-x-1 mb-4"
                    inputStyle="mt-2 w-[9.5%] transition block rounded-md border-gray-300 focus:border-blue-500 placeholder-slate-400 text-xs" />
                <button type="button"
                    className="rounded-md mt-6 bg-blue-500 hover:bg-blue-600 aria-disabled:bg-blue-300 w-full h-10 text-white transition aria-disabled:cursor-not-allowed"
                    onClick={onSubmit}
                    disabled={isLoading || code.length < 10}
                    aria-disabled={isLoading || code.length < 10}>
                    验证
                </button>
            </div>
        </div>
    )
}

export default connect(({ loading }: { loading: any }) => ({
    isLoading: loading.effects['login/loginWithRecoveryCode']
}))(LoginWithRecoveryCodePage)