import { Button } from "@/components/ui/button";
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Input, InputLabel } from "@/components/ui/input";
import { Fragment, useEffect, useState } from "react"
import { useForm } from "react-hook-form"

interface InputOrgDialogProps {
    isOpen: boolean,
    isProcessing?: boolean,
    currentId?: string
    onClose?: () => void,
    onConfirm?: (actionType: 'create' | 'update', value: any) => Promise<boolean>;
}

const ACTION_TEXTS = {
    'update': { title: '编辑', btn: '保存' },
    'create': { title: '新建', btn: '确认' },
}

const InputOrgDialog = ({
    isOpen,
    isProcessing,
    currentId,
    onClose,
    onConfirm
}: InputOrgDialogProps) => {
    const [actionType, setActionType] = useState<'create' | 'update'>('create')

    const { register, handleSubmit, reset, formState: { errors } } = useForm({

    })

    useEffect(() => {
        if (!isOpen) return
        const type = typeof currentId === 'undefined' ? 'create' : 'update'
        setActionType(type)
    }, [isOpen])

    useEffect(() => {

    }, [])

    const restValues = () => {
        reset()
    }

    const onSubmit = async (value: any) => {
        if (onConfirm && await onConfirm(actionType, value)) {
            restValues()
        }
    }

    const handleOpenChange = (open: boolean) => {
        if (!open) {
            onClose && onClose()
        }

        restValues()
    }

    return (
        <Dialog open={isOpen} onOpenChange={handleOpenChange}>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>
                        {ACTION_TEXTS[actionType].title}组织
                    </DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <input value={currentId} type="hidden" {...register('id')} />
                    <div>
                        <div className="flex flex-col gap-y-8 w-full">
                            <InputLabel text="组织名称" required>
                                <Input type="text"
                                    placeholder="请输入组织名称"
                                    disabled={isProcessing}
                                    aria-invalid={errors.name ? 'true' : 'false'}
                                    {...register('name', { required: true })} />
                            </InputLabel>
                            <InputLabel text="组织标识" required>
                                <Input type="text"
                                    placeholder="请输入组织标识"
                                    disabled={isProcessing}
                                    aria-invalid={errors.code ? 'true' : 'false'}
                                    {...register('code', { required: true })} />
                            </InputLabel>
                            <InputLabel text="组织描述">
                                <textarea
                                    className="w-full border-none rounded bg-gray-100 dark:bg-gray-700 text-sm focus:bg-white transition duration-300 placeholder:text-gray-300"
                                    placeholder="请输入组织描述"
                                    disabled={isProcessing}
                                    {...register('description')} />
                            </InputLabel>
                        </div>
                    </div>
                    <DialogFooter className="pt-8">
                        <Button variant="secondary"
                            onClick={() => handleOpenChange(false)}
                            disabled={isProcessing}
                        >
                            取消
                        </Button>
                        <Button type="submit"
                            disabled={isProcessing}>
                            {ACTION_TEXTS[actionType].btn}
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}


export default InputOrgDialog