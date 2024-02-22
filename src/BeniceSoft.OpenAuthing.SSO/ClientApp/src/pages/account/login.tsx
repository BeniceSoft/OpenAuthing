import Lottie from "lottie-react";
import AnimationData from '@/assets/animations/secure-login.json'
import { useForm } from "react-hook-form";
import React, { useEffect } from "react";
import { useSearchParams, connect, useDispatch } from 'umi'
import { LoginModelState } from "@/models/login";
import { ExternalLoginProvider } from "@/@types/auth";
import { getSearchParam } from "@/lib/misc";

function renderExternalProviderIcon(providerName: string) {
    switch (providerName.toLowerCase()) {
        case "feishu":
            return (
                <svg className="icon" width="32px" height="32px" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg">
                    <path
                        d="M243.413333 186.538667h372.010667s130.944 144.64 103.978667 264.106666-182.485333 77.568-182.485334 77.568-156.373333-241.066667-295.722666-335.488a3.370667 3.370667 0 0 1 2.261333-6.186666z"
                        fill="#01D7BB" />
                    <path
                        d="M350.762667 650.709333a3.370667 3.370667 0 0 1 0-6.741333 505.770667 505.770667 0 0 0 218.581333-147.328 317.482667 317.482667 0 0 1 229.290667-119.04 305.152 305.152 0 0 1 118.016 25.856 2.816 2.816 0 0 1 0 4.48 542.848 542.848 0 0 0-72.533334 121.941333 561.962667 561.962667 0 0 1-56.192 107.904 327.04 327.04 0 0 1-39.893333 47.786667l-69.674667 56.192z"
                        fill="#0C39A0" />
                    <path
                        d="M106.282667 389.973333a5.077333 5.077333 0 0 1 8.533333-4.48c52.266667 49.450667 331.008 299.52 572.629333 289.408a155.093333 155.093333 0 0 0 107.904-56.192c48.341333-56.192-92.714667 186.026667-319.744 214.101334a528.256 528.256 0 0 1-350.677333-75.861334 42.666667 42.666667 0 0 1-18.56-35.413333z"
                        fill="#326EFF" />
                </svg>
            );
        default:
            return (
                <></>
            )
    }
}

interface LoginProps {
    externalLoginProvidersLoading: boolean
    isLoggingIn: boolean
    externalLoginProviders?: ExternalLoginProvider[]
}

const Login: React.FC<LoginProps> = (props: LoginProps) => {
    const { register, formState: { errors, isValid }, handleSubmit } = useForm()
    const dispatch = useDispatch()
    const [searchParams] = useSearchParams()

    const { externalLoginProvidersLoading, externalLoginProviders, isLoggingIn } = props;

    useEffect(() => {
        dispatch({
            type: 'login/fetchLoginProviders'
        })
    }, []);

    console.log('isLoggingIn', isLoggingIn)

    const onSubmit = (data: any) => {
        dispatch({
            type: 'login/login',
            payload: {
                ...data,
                returnUrl: getSearchParam(searchParams, 'returnUrl') ?? '',
                rememberMe: true
            }
        })
    }

    return (

        <div className="rounded-lg shadow-[0_8px_24px_0px_rgba(45,46,50,.15)] w-[900px] md:w-[860px] h-[500px] overflow-hidden bg-white">
            <div className="columns-2 h-full gap-0">
                <div className="w-full h-full flex items-center justify-center">
                    <Lottie className="w-4/5" animationData={AnimationData} loop={true} />
                </div>
                <div className="w-full h-full px-8">
                    <div className="text-center p-10 pt-14">
                        <h1 className="text-3xl">OpenAuthing</h1>
                    </div>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <label className="block">
                            <span className="text-gray-700 text-sm">用户名</span>
                            <input type="text"
                                disabled={isLoggingIn}
                                placeholder="请输入用户名" required
                                aria-invalid={errors.userName ? "true" : "false"}
                                className="mt-2 transition block w-full rounded-md border-gray-300 focus:border-blue-500 placeholder-slate-400 text-sm"
                                {...register('userName', { required: true })} />
                        </label>
                        <label className="block mt-4">
                            <span className="text-gray-700 text-sm">密码</span>
                            <input type="password"
                                disabled={isLoggingIn}
                                placeholder="请输入密码" required
                                aria-invalid={errors.password ? "true" : "false"}
                                className="mt-2 transition block w-full rounded-md border-gray-300 foucs:border-blue-500 placeholder-slate-400 text-sm"
                                {...register('password', { required: true })} />
                        </label>
                        <button type="submit"
                            className="rounded-md mt-6 bg-blue-500 hover:bg-blue-600 aria-disabled:bg-blue-300 w-full h-10 text-white transition aria-disabled:cursor-not-allowed"
                            aria-disabled={isLoggingIn || !isValid}
                            disabled={isLoggingIn || !isValid}>
                            登录
                        </button>
                    </form>
                    {(externalLoginProviders && externalLoginProviders.length > 0) &&
                        <div className="mt-4 mb-2">
                            <p className="text-center text-sm text-gray-400">支持使用以下外部账号登录</p>
                            <div className="space-x-4">
                                {externalLoginProviders.map((element: any) => (
                                    <button key={element.name} title={`Login with ${element.displayName} account`}
                                        value={element.name}
                                        className="transition duration-300 rounded-full w-[40px] h-[40px] flex bg-gray-50 hover:bg-gray-100 items-center justify-center">
                                        {renderExternalProviderIcon(element.providerName)}
                                    </button>
                                ))}
                            </div>
                        </div>
                    }
                </div>
            </div>
        </div>
    )
}

export default connect(({ loading, login }: { loading: any, login: LoginModelState }) => ({
    ...login,
    externalLoginProvidersLoading: loading.effects['login/fetchLoginProviders'],
    isLoggingIn: loading.effects['login/login']
}))(Login)