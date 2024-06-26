import ContentBlock from "@/components/ContentBlock";
import Mask from "@/components/Mask";
import Spin from "@/components/Spin";
import { Button } from "@/components/ui/button";
import { Input, InputLabel } from "@/components/ui/input";
import AccountService from "@/services/account.service";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { FormattedMessage, useIntl, useRequest } from "umi";

const ProfilePage = function (props: any) {
    const intl = useIntl()
    const { loading, data: profile } = useRequest(AccountService.getProfile)
    const { register, formState: { errors, isValid, isSubmitting }, handleSubmit, reset } = useForm()

    useEffect(() => {
        reset(profile)
    }, [profile])

    const onSubmit = (data: any) => {
        console.log(data)
    }

    return (
        <div className="grid gap-y-6">
            <ContentBlock title={intl.formatMessage({ id: 'settings.profile.title' })}>
                <div className="relative">
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.username' })} required={true}>
                                <Input type="text"
                                    placeholder=""
                                    {...register('userName', { required: true })} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.nickname' })} required={true}>
                                <Input type="text"
                                    placeholder=""
                                    {...register('nickname', { required: true })} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.phonenumber' })}>
                                <Input type="text"
                                    disabled={true}
                                    {...register('phoneNumber')} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.emailaddress' })}>
                                <Input type="text"
                                    disabled={true}
                                    {...register('emailAddress')} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.gender' })}>
                                <Input type="text"
                                    placeholder=""
                                    {...register('gender')} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.jobtitle' })}>
                                <Input type="text"
                                    disabled={true}
                                    {...register('jobTitle')} />
                            </InputLabel>
                            <InputLabel text={intl.formatMessage({ id: 'settings.profile.input.creationtime' })}>
                                <Input type="text"
                                    disabled={true}
                                    {...register('creationTime')} />
                            </InputLabel>
                            <div className="lg:col-span-2">

                                <Button type="submit" disabled={isSubmitting || !isValid}>
                                    <FormattedMessage id="settings.profile.button.save" />
                                </Button>
                            </div>
                        </div>
                    </form>

                    {loading && <Mask loadingText={intl.formatMessage({ id: "settings.profile.loading" })} />}
                </div>
            </ContentBlock>
        </div>
    )
}

export default ProfilePage