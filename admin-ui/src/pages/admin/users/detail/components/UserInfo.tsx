import { UserDetail } from "@/@types/user"
import { Button } from "@/components/ui/button"
import { Input, InputLabel } from "@/components/ui/input"
import { useForm } from "react-hook-form"


type UserInfoProps = {
    userInfo: UserDetail
}


export default ({
    userInfo
}: UserInfoProps) => {

    const { register, reset, handleSubmit, formState: { errors, isValid } } = useForm({
        defaultValues: {
            ...userInfo
        }
    })

    const onSubmit = (value: any) => {

    }

    return (
        <div className="space-y-4">
            <div>
                <h2 className="text-lg font-medium">账号信息</h2>
                <div className="grid grid-cols-3 gap-x-8 gap-y-4 py-2 text-gray-500">
                    <InputLabel text="创建时间">
                        <Input disabled={true} variant="solid" />
                    </InputLabel>
                    <InputLabel text="最后登录时间">
                        <Input disabled={true} variant="solid" />
                    </InputLabel>
                    <InputLabel text="最后登录 IP">
                        <Input disabled={true} variant="solid" />
                    </InputLabel>
                    <InputLabel text="登录次数">
                        <Input disabled={true} variant="solid" />
                    </InputLabel>
                </div>
            </div>
            <div>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <h2 className="text-lg font-medium">个人信息</h2>
                    <div className="grid grid-cols-3 gap-x-8 gap-y-4 py-2 text-gray-500">
                        <InputLabel text="用户名" required>
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn}
                                {...register('userName', { required: true })} />
                        </InputLabel>
                        <InputLabel text="昵称" required>
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn}
                                {...register('nickname', { required: true })} />
                        </InputLabel>
                        <InputLabel text="性别">
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn} />
                        </InputLabel>
                        <InputLabel text="生日">
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn} />
                        </InputLabel>
                        <InputLabel text="手机号" required>
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn}
                                {...register('phoneNumber', { required: true })} />
                        </InputLabel>
                        <InputLabel text="邮箱">
                            <Input variant="solid"
                                disabled={userInfo.isSystemBuiltIn} />
                        </InputLabel>
                        <div className="col-span-3 space-x-4">
                            <Button type="submit"
                                disabled={userInfo.isSystemBuiltIn || isValid}>
                                保存
                            </Button>
                            <Button variant="secondary" onClick={() => reset()}
                                disabled={userInfo.isSystemBuiltIn}>
                                重置
                            </Button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    )
}