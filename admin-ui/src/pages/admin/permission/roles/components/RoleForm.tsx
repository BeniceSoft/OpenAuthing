import { useForm } from "react-hook-form"

export interface RoleFormProps {
    initValue?: any
    isBusy?: boolean
    disabled?: boolean
    onSubmit?: (value: any) => Promise<void>
}

const RoleForm = ({
    initValue,
    isBusy = false,
    disabled = false,
    onSubmit
}: RoleFormProps) => {
    const { handleSubmit, register, formState: { errors } } = useForm({ values: initValue })

    const onValid = async (value: any) => {
        console.log(value)

        onSubmit && await onSubmit(value)
    }

    return (
        <form onSubmit={handleSubmit(onValid)}>
            <div className="grid grid-cols-2 gap-x-20">
                <label className="flex relative flex-col gap-y-2 text-sm pb-8">
                    <span className="text-gray-600 after:content-['*'] after:text-red-600">角色显示名</span>
                    <input type="text"
                        disabled={disabled || isBusy}
                        className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                        aria-invalid={errors.displayName ? 'true' : 'false'}
                        {...register('displayName', { required: true })} />
                    {errors.displayName &&
                        <span className="absolute bottom-2 left-0 text-xs text-red-500">
                            请输入「角色显示名」
                        </span>
                    }
                </label>
                <label className="flex relative flex-col gap-y-2 text-sm pb-8">
                    <span className="text-gray-600 after:content-['*'] after:text-red-600">角色名</span>
                    <input type="text"
                        disabled={disabled || isBusy}
                        className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                        aria-invalid={errors.name ? 'true' : 'false'}
                        {...register('name', { required: true })} />
                    {errors.name &&
                        <span className="absolute bottom-2 left-0 text-xs text-red-500">
                            请输入「角色名」
                        </span>
                    }
                </label>
                <label className="flex relative flex-col gap-y-2 text-sm">
                    <span className="text-gray-600">描述</span>
                    <textarea rows={4} maxLength={200}
                        disabled={disabled || isBusy}
                        className="border-none rounded bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 aria-invalid:ring-red-500"
                        aria-invalid={errors.description ? 'true' : 'false'}
                        {...register('description')} />
                </label>
            </div>
            <div className="mt-6">
                <button type="submit"
                    disabled={disabled || isBusy}
                    className="rounded bg-blue-600 text-white py-1.5 px-6 text-sm transition-colors hover:bg-blue-700 disabled:cursor-not-allowed disabled:bg-blue-300">
                    {!disabled && isBusy ?
                        <span className="flex gap-x-1 items-center">
                            <svg className="animate-spin h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                            </svg>
                            保存中
                        </span>
                        : '保存'}
                </button>
            </div>
        </form>
    )
}

export default RoleForm