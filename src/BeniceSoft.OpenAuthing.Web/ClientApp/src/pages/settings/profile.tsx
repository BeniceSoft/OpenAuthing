import ContentBlock from "@/components/ContentBlock";
import Spin from "@/components/Spin";
import { Button } from "@/components/ui/button";
import { Input, InputLabel } from "@/components/ui/input";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useModel } from "umi";

const ProfilePage = function (props: any) {
    const { register, formState: { errors, isValid }, handleSubmit, reset } = useForm()

    const { loading, profile, fetch } = useModel('settings.profile')

    useEffect(() => {
        fetch()
    }, [])

    useEffect(() => {
        reset({ ...profile })
    }, [profile])

    const onSubmit = (data: any) => {
        console.log(data)
    }

    return (
        <div className="grid gap-y-6">
            <ContentBlock title="基础信息">
                <Spin spinning={loading ?? false}>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className="grid grid-col-1 lg:grid-cols-2 gap-8">
                            <InputLabel text="用户名" required={true}>
                                {/* <span className="block text-sm mb-1 after:content-['*'] after:text-red-600"></span> */}
                                <Input type="text"
                                    placeholder=""
                                    {...register('userName', { required: true })} />
                            </InputLabel>
                            <InputLabel text="昵称" required={true}>
                                <Input type="text"
                                    placeholder=""
                                    {...register('nickname', { required: true })} />
                            </InputLabel>
                            <InputLabel text="手机号码">
                                <Input type="text"
                                    disabled={true}
                                    {...register('phoneNumber')} />
                            </InputLabel>
                            <InputLabel text="邮箱地址">
                                <Input type="text"
                                    disabled={true}
                                    {...register('emailAddress')} />
                            </InputLabel>
                            <InputLabel text="性别">
                                <Input type="text"
                                    placeholder=""
                                    {...register('gender')} />
                            </InputLabel>
                            <InputLabel text="职务">
                                <Input type="text"
                                    disabled={true}
                                    {...register('jobTitle')} />
                            </InputLabel>
                            <InputLabel text="创建日期">
                                <Input type="text"
                                    disabled={true}
                                    {...register('creationTime')} />
                            </InputLabel>
                            <div className="lg:col-span-2">
                                <Button type="submit"
                                    disabled={!isValid}
                                    className="px-6">
                                    保存
                                </Button>
                            </div>
                        </div>
                    </form>
                </Spin>
            </ContentBlock>
        </div>
    )
}

export default ProfilePage