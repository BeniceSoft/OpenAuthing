import { Button } from "@/components/ui/button"
import { Dialog, DialogClose, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Input, InputErrorMessage, InputLabel } from "@/components/ui/input"
import React, { useImperativeHandle, useState } from "react"
import { useForm } from "react-hook-form"

export type TreeNodeModalRef = {
    open: (currentNode?: any, parentNode?: any) => void
}

export type TreeNodeModalProps = {
    onSave: (data: any) => void
}

const TreeNodeModal = React.forwardRef<TreeNodeModalRef, TreeNodeModalProps>(({
    onSave
}, ref) => {
    const [open, setOpen] = useState<boolean>()
    const [currentNode, setCurrentNode] = useState<any>()
    const [parentNode, setParentNode] = useState<any>()
    const { handleSubmit, reset, register, setValue, formState: { isValid, errors } } = useForm({ mode: 'all' })

    useImperativeHandle(ref, () => ({
        open: (currentNode, parentNode) => {
            setOpen(true)
            setParentNode(parentNode)
            setCurrentNode(currentNode)

            setValue('name', currentNode?.name)
            setValue('key', currentNode?.key)
            setValue('value', currentNode?.value)
        }
    }))

    const onClose = () => {
        reset()
        setOpen(false)
    }

    const onSubmit = (value: any) => {
        const data = {
            ...value,
            parentNode,
            currentNode
        }

        onSave(data)

        onClose()
    }

    return (
        <Dialog open={open} onOpenChange={x => !x && onClose()}>
            <DialogContent className="min-w-[600px] max-w-[600px]">
                <DialogHeader>
                    <DialogTitle>
                        新增节点
                    </DialogTitle>
                </DialogHeader>
                <form>
                    <input type="hidden" {...register("parentPath")} />
                    <div>
                        <div className="mb-8 flex flex-col gap-y-6">
                            <InputLabel text="名称" required>
                                <Input type="text" placeholder="请输入节点名称"
                                    invalid={!!errors.name}
                                    {...register("name", { required: true })} />
                            </InputLabel>
                            <InputLabel text="键" required>
                                <Input type="text" placeholder="请输入节点键"
                                    invalid={!!errors.key}
                                    {...register("key", { required: true, pattern: /^[A-Za-z0-9-\_]+$/ })} />
                                {errors.key?.type === 'pattern' && <InputErrorMessage message="只允许包含英文字母、数字、下划线 _、横线 -" />}
                            </InputLabel>
                            <InputLabel text="值">
                                <Input type="text" placeholder="请输入节点值" {...register("value")} />
                            </InputLabel>
                        </div>
                        <DialogFooter>
                            <DialogClose asChild>
                                <Button type="button" variant="secondary">取消</Button>
                            </DialogClose>
                            <Button type="button" disabled={!isValid}
                                onClick={handleSubmit(onSubmit)}>确定</Button>
                        </DialogFooter>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    )
})

export default TreeNodeModal