import { useForm } from "react-hook-form";
import React, { useEffect } from "react";
import { useSearchParams, FormattedMessage, FormattedHTMLMessage, useModel, Link } from 'umi'
import { ExternalLoginProvider, LoginWithPasswordModel } from "@/@types/auth";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import { HSTogglePassword } from 'preline/preline'
import useReturnUrl from "@/hooks/useReturnUrl";

function renderExternalProviderIcon(providerName: string) {
    switch (providerName.toLowerCase()) {
        case "feishu":
            return (
                <svg className="icon" width="16px" height="16px" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg">
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

type LoginPageProps = {
    externalLoginProvidersLoading: boolean
    isLoggingIn: boolean
    externalLoginProviders?: ExternalLoginProvider[]
}

const LoginPage: React.FC<LoginPageProps> = (props: LoginPageProps) => {
    const { register, formState: { isValid, isSubmitting }, handleSubmit } = useForm<LoginWithPasswordModel>()
    const returnUrl = useReturnUrl()

    useEffect(() => {
        HSTogglePassword.autoInit()
    }, [])

    const { loginWithPassword } = useModel('account.login', model => ({
        loginWithPassword: model.loginWithPassword
    }))

    const onSubmit = async (data: LoginWithPasswordModel) => {
        await loginWithPassword({
            ...data,
            returnUrl
        })
    }

    return (
        <div>
            <h1 className="text-2xl font-semibold text-neutral-800 dark:text-neutral-200">
                <FormattedMessage id="account.login.title.text" />
            </h1>
            <p className="mt-1 text-sm text-gray-400 dark:text-neutral-500">
                <FormattedMessage id="account.login.desc.text" />
            </p>
            <form className="my-5" onSubmit={handleSubmit(onSubmit)}>
                <div className="space-y-5">
                    <div>
                        <label htmlFor="username" className="block text-gray-700 text-sm mb-2 font-medium dark:text-white">
                            <FormattedMessage id="account.login.input.username.label" />
                        </label>
                        <input type="text"
                            id="username"
                            className="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600"
                            placeholder="administrator"
                            {...register('userName', { required: true })} />
                    </div>
                    <div>
                        <div className="flex justify-between">
                            <label htmlFor="password" className="block text-gray-700 text-sm mb-2 font-medium dark:text-white">
                                <FormattedMessage id="account.login.input.password.label" />
                            </label>
                            <Link to={{ pathname: "/account/reset-password", search: "?returnUrl=" + returnUrl }} className="text-xs text-gray-500 hover:underline">
                                <FormattedMessage id="account.login.link.forgotPassword.text" />
                            </Link>
                        </div>
                        <div className="relative">
                            <input type="password"
                                id="password"
                                className="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-slate-900 dark:border-gray-700 dark:text-gray-400 dark:focus:ring-gray-600"
                                placeholder="**********"
                                {...register('password', { required: true })} />
                            <button type="button"
                                data-hs-toggle-password='{"target":"#password"}'
                                className="absolute top-0 end-0 p-4 rounded-e-md">
                                <EyeOffIcon className="flex-shrink-0 size-3.5 text-gray-400 dark:text-neutral-600 w-4 h-4 hs-password-active:hidden" />
                                <EyeIcon className="hidden flex-shrink-0 size-3.5 text-gray-400 dark:text-neutral-600 w-4 h-4 hs-password-active:block" />
                            </button>
                        </div>
                    </div>
                    <div className="flex">
                        <input type="checkbox"
                            id="rememberMe"
                            className="shrink-0 mt-0.5 border-gray-200 rounded text-blue-600 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-gray-800 dark:border-gray-700 dark:checked:bg-blue-500 dark:checked:border-blue-500 dark:focus:ring-offset-gray-800"
                            {...register('rememberMe', { required: false })} />
                        <label htmlFor="rememberMe" className="text-sm text-gray-500 ms-3 dark:text-gray-400">
                            <FormattedMessage id="account.login.input.rememberMe.label" />
                        </label>
                    </div>

                    <div className="pt-0">
                        <button type="submit"
                            className="w-full p-3 inline-flex justify-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
                            aria-disabled={isSubmitting || !isValid}
                            disabled={isSubmitting || !isValid}>
                            <FormattedMessage id="account.login.button.login.text" />
                        </button>
                    </div>
                    <p className="text-sm text-gray-600">
                        <FormattedHTMLMessage id="account.login.aggreement.html" />
                    </p>
                    {/* <div className="space-y-2">
                        <div className="flex items-center text-gray-400 font-medium text-xs before:content-[''] before:flex-1 before:border-gray-300 before:mr-6 before:border-t after:content-[''] after:flex-1 after:border-gray-300 after:ml-6 after:border-t dark:text-neutral-500 dark:before:border-neutral-700 dark:after:border-neutral-700">
                            <FormattedMessage id="account.login.orSignInWith.text" />
                        </div>
                        <div className="flex gap-2">

                        </div>
                    </div> */}
                </div>
            </form >
        </div >
    )
}

export default LoginPage