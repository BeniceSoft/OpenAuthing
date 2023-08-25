import { useSearchParams, history } from "umi"
import { DataTypes } from ".."
import { Suspense, lazy, useEffect, useState } from "react"
import { Badge } from "@/components/ui/badge"
import { Input, InputLabel } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Controller, useForm } from "react-hook-form"
import { X } from "lucide-react"
import { confirm } from "@/components/Modal/ConfirmModal"

const ListData = lazy(() => import("./components/list-data"))
const TreeData = lazy(() => import("./components/tree-data"))
const StringData = lazy(() => import("./components/string-data"))

function renderDataInputComponent(type: string, props: any) {
    switch (type) {
        case 'TREE':
            return <TreeData {...props} />
        case 'ARRAY':
            return <ListData {...props} />
        case 'STRING':
            return <StringData {...props} />
        default:
            return "unknown"
    }
}

export default () => {
    const [searchParams, setSearchParams] = useSearchParams()
    const { register, handleSubmit, setValue, getValues, control, formState: { errors, isValid } } = useForm({ mode: 'all' })
    const [actions, setActions] = useState<string[]>([])
    const [type, setType] = useState<string>("")

    useEffect(() => {
        let dataType = searchParams.get('type') || ''
        if (!DataTypes[dataType]) {
            dataType = Object.keys(DataTypes)[0]
        }

        setType(dataType)
        setValue('type', dataType)
    }, [searchParams])

    const onCancel = () => {
        confirm({
            title: '确定离开此页面？',
            content: '本次修改未保存，确定离开？',
            onOK: () => history.back(),
            okButton: {
                text: '确定离开',
                className: 'bg-transparent text-secondary-foreground'
            },
            closeButton: {
                text: '继续配置',
                className: 'bg-primary text-primary-foreground',
            }
        })
    }

    const handleCreate = (value: any) => {
        const data = {
            ...value,
            actions
        }
        console.log('create: ', data)
    }

    const onAddAction = () => {
        const action = getValues()['actions']
        if (action) {
            setValue('actions', '')
            if (actions.findIndex(x => x === action) >= 0) return
            setActions(actions.concat(action))
        }
    }

    const onRemoveAction = (index: number) => {
        setActions(actions.filter((_, i) => index !== i))
    }

    return (
        <div className="w-full">
            <div className="mb-2">
                <span onClick={onCancel}
                    className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-4 h-4">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
                    </svg>
                    返回
                </span>
            </div>
            <div className="mb-7 flex items-center gap-x-1">
                <h1 className="text-xl font-semibold">创建数据资源</h1>
                <Badge>{DataTypes[type]?.title}</Badge>
            </div>
            <main className="w-full flex flex-col gap-y-6 mt-4">
                <form onSubmit={handleSubmit(handleCreate)} className="grid grid-cols-2 gap-x-16 gap-y-8">
                    <input type="hidden" {...register("type")} />
                    <InputLabel text="资源名称" required>
                        <Input type="text" placeholder="请输入资源名称" variant="solid"
                            invalid={!!errors.name}
                            {...register("name", { required: true })} />
                    </InputLabel>
                    <InputLabel text="资源标识" required>
                        <Input type="text" placeholder="请输入资源标识，填写一个名称，例如：order" variant="solid"
                            invalid={!!errors.code}
                            {...register("code", { required: true })} />
                    </InputLabel>
                    <InputLabel text="资源描述" >
                        <Input type="text" placeholder="请输入资源描述" variant="solid"
                            {...register("description")} />
                    </InputLabel>

                    <div className="col-span-2">
                        <Controller name="data"
                            control={control}
                            rules={{ required: true }}
                            render={({ field }) => (
                                <Suspense fallback="Loading">
                                    {renderDataInputComponent(type, field)}
                                </Suspense>
                            )} />
                    </div>

                    <InputLabel text="资源操作" required>
                        <Input variant="solid" className="flex-1" type="text" placeholder="请输入操作名称(回车添加)"
                            {...register("actions", { validate: { required: _ => actions.length > 0 } })}
                            invalid={!!errors.actions}
                            onKeyDown={e => {
                                if (e.key === 'Enter') {
                                    e.preventDefault()
                                    onAddAction();
                                }
                            }}
                        />
                        <div className="mt-3 flex gap-x-2">
                            {actions.map((action, index) => (
                                <div key={index} className="flex gap-x-1 items-center px-2 py-1 text-xs bg-secondary rounded">
                                    {action}
                                    <X className="w-3 h-3 cursor-pointer" onClick={() => onRemoveAction(index)} />
                                </div>
                            ))}
                        </div>
                    </InputLabel>

                    <div className="col-span-2 flex items-center gap-x-4">
                        <Button type="submit" disabled={!isValid}>创建</Button>
                        <Button type="button" variant="secondary" onClick={onCancel}>取消</Button>
                    </div>
                </form>
            </main>
        </div>
    )
}