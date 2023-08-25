import { Dialog, DialogFooter, DialogHeader, DialogContent, DialogTitle } from "@/components/ui/dialog"
import React, { useEffect, useState } from "react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { Input, InputLabel } from "@/components/ui/input"
import { useForm } from "react-hook-form"
import { Button } from "@/components/ui/button"

interface CreateUserDialogProps extends DialogPrimitive.DialogProps {
    onCreate?: (value: any) => void
    creating?: boolean
}

const CreateUserDialog: React.FC<CreateUserDialogProps> = ({
    open,
    onOpenChange,
    onCreate,
    creating
}) => {
    const { register, handleSubmit, reset } = useForm()

    useEffect(() => {
        if(open === false){
            reset({})
        }
    }, [open])

    const handleOpenChange = (open: boolean) => {
        onOpenChange && onOpenChange(open)
    }

    const onSubmit = (value: any) => {
        try {
            onCreate && onCreate(value)
        }
        finally {
            handleOpenChange(false)
        }
    }

    return (
        <Dialog open={open} onOpenChange={handleOpenChange}>
            <DialogContent className="sm:max-w-xl">
                <DialogHeader>
                    <DialogTitle>创建用户</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="grid grid-cols-1 gap-y-4">
                        <InputLabel text="用户名" required={true}>
                            <Input type="text"
                                disabled={creating}
                                autoComplete="disabled"
                                placeholder="请输入用户名（唯一）"
                                {...register('userName', { required: true })} />
                        </InputLabel>
                        <InputLabel text="手机号码" required={true}>
                            <Input type="text"
                                disabled={creating}
                                autoComplete="disabled"
                                placeholder="请输入手机号码（唯一）"
                                {...register('phoneNumber', { required: true })} />
                        </InputLabel>
                        <InputLabel text="初始密码" required={true}>
                            <Input type="password"
                                disabled={creating}
                                autoComplete="disabled"
                                placeholder="请输入初始密码"
                                {...register('password', { required: true })} />
                        </InputLabel>
                    </div>
                    <DialogFooter className="mt-8 flex gap-x-2">
                        <Button type="button" variant="secondary"
                            disabled={creating}
                            onClick={() => handleOpenChange(false)}>取消</Button>
                        <Button type="submit"
                            disabled={creating}>确认</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}

export default CreateUserDialog