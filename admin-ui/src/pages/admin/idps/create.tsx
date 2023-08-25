
import { idPTemplate } from '@/@types/identityProviderTemplate'
import Spin from '@/components/Spin'
import { CreateIdPModelState } from '@/models/createIdP'
import React, { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { connect, useParams, history, Dispatch } from 'umi'


interface CreateIdPPageProps {
    dispatch: Dispatch
    loading?: boolean
    template?: idPTemplate
}

const CreateIdPPage: React.FC<CreateIdPPageProps> = ({
    dispatch,
    loading,
    template
}: CreateIdPPageProps) => {
    const { providerName } = useParams()
    const { register, formState: { errors } } = useForm()

    useEffect(() => {
        dispatch({
            type: 'createIdP/fetch',
            payload: { providerName }
        })

    }, [providerName])


    return (
        <div className="w-full h-full flex flex-col">
            <div className="mb-2">
                <span onClick={history.back}
                    className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                    </svg>
                    返回
                </span>
            </div>

            <Spin spinning={loading ?? false}>
                {template &&
                    <>
                        <div className="flex gap-x-4 items-center mb-8">
                            <div className="flex-shrink-0 w-16 h-16 rounded overflow-hidden">
                                <img src={template.logo} alt="logo"
                                    className="w-full h-full" />
                            </div>
                            <div className="w-full">
                                <h1 className="text-xl font-semibold mb-3">{template.title}</h1>
                                <p className="text-sm text-gray-400">{template.description}</p>
                            </div>
                        </div>

                        <div className="flex flex-col gap-y-8">
                            <form>
                                <div className="">
                                    <h3 className="mb-6 font-medium">基础信息</h3>
                                    <div className="grid grid-cols-2 gap-x-20 gap-y-4">
                                        <label className="flex flex-col gap-y-2 relative">
                                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                                唯一标识
                                            </span>
                                            <input type="text"
                                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                                                aria-invalid={errors.name ? 'true' : 'false'}
                                                {...register('name', { required: true })} />
                                            {errors.name &&
                                                <span className="absolute bottom-1.5 left-0 text-sm text-red-500">
                                                    请输入「唯一标识」
                                                </span>
                                            }
                                        </label>
                                        <label className="flex flex-col gap-y-2 relative">
                                            <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                                显示名称
                                            </span>
                                            <input type="text"
                                                className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                                                aria-invalid={errors.displayName ? 'true' : 'false'}
                                                {...register('displayName', { required: true })} />
                                            {errors.displayName &&
                                                <span className="absolute bottom-1.5 left-0 text-sm text-red-500">
                                                    请输入「显示名称」
                                                </span>
                                            }
                                        </label>

                                        {template.fields?.map(field => (
                                            <label className="flex flex-col gap-y-2 relative">
                                                <span className="text-gray-600 dark:text-gray-400 text-sm after:content-['*'] after:ml-0.5 after:text-red-500">
                                                    {field.label}
                                                </span>
                                                <input type={field.type}
                                                    className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                                                    aria-invalid={errors[field.name] ? 'true' : 'false'}
                                                    {...register(field.name, { required: true })} />
                                                {errors.displayName &&
                                                    <span className="absolute bottom-1.5 left-0 text-sm text-red-500">
                                                        请输入「显示名称」
                                                    </span>
                                                }
                                            </label>
                                        ))}
                                    </div>
                                </div>

                                <div className="flex gap-x-2 mt-8">
                                    <button type="submit"
                                        className="px-4 py-1.5 rounded bg-blue-600 text-white text-sm">
                                        保存
                                    </button>
                                    <button type="reset"
                                        className="px-4 py-1.5 rounded bg-gray-100 text-red-600 text-sm">
                                        重置
                                    </button>
                                </div>
                            </form>
                        </div>
                    </>
                }
            </Spin>

        </div>
    )
}

export default connect(({ loading, createIdP }: { loading: any, createIdP: CreateIdPModelState }) => ({
    loading: loading.effects['createIdP/fetch'],
    ...createIdP
}))(CreateIdPPage)