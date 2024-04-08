import ContentBlock from '@/components/ContentBlock'
import { Edit3Icon, LinkIcon, MailIcon, PhoneIcon, UnlinkIcon } from 'lucide-react'
import React from 'react'
import { FormattedMessage, useIntl } from 'umi'

const LinkButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button type="button"
        ref={ref}
        className="flex items-center gap-x-1 text-sm font-medium text-blue-600 hover:text-blue-700 transition-colors">
        <LinkIcon className="w-4 h-4" />
        <FormattedMessage id="common.bind" />
    </button>
))

const EditButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button className="flex items-center gap-x-1 text-sm font-medium text-blue-600 hover:text-blue-700 transition-colors">
        <Edit3Icon className="w-4 h-4" />
        <FormattedMessage id="common.edit" />
    </button>
))

const UnLinkButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button className="flex items-center gap-x-1 text-sm">
        <UnlinkIcon className="text-orange-600 w-4 h-4" />
        <span className="text-gray-600 hover:text-gray-800 font-medium transition-colors">
            <FormattedMessage id="common.unbind" />
        </span>
    </button>
))

const AccountSettingsPage: React.FC = () => {
    const intl = useIntl()

    return (
        <div className="flex flex-col gap-y-8">
            <ContentBlock title={intl.formatMessage({ id: "settings.account.binding.emailaddress&phonenumber" })}>
                <div className="flex flex-col gap-y-6">
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 p-2 bg-slate-100 rounded-md">
                            <MailIcon className="w-5 h-5 stroke-slate-600" />
                        </div>
                        <p className="flex-1 text-sm">
                            <span className="font-medium"><FormattedMessage id="settings.account.binding.emailaddress" /></span>
                        </p>
                        <div className="flex gap-x-2">
                            <LinkButton />
                        </div>
                    </div>
                    <div className="flex items-center justify-center gap-x-4">
                        <div className="w-9 h-9 p-2 bg-slate-100 rounded-md">
                            <PhoneIcon className="w-5 h-5 stroke-slate-600" />
                        </div>
                        <p className="flex-1 text-sm">
                            <span className='font-medium'>
                                <FormattedMessage id="settings.account.binding.phonenumber" />
                            </span>
                            <span className="text-blue-600 text-sm before:content-[':'] before:mx-1 before:text-slate-600">
                                130****0000
                            </span>
                        </p>
                        <div className="flex gap-x-4">
                            <UnLinkButton />
                            <EditButton />
                        </div>
                    </div>
                </div>
            </ContentBlock>
            <ContentBlock title={intl.formatMessage({ id: "settings.account.binding.thirepartyaccount" })}>
                <div className="flex flex-col gap-y-6">
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 bg-slate-100 rounded">
                            <img className="w-full h-full"
                                src="https://files.authing.co/authing-console/social-connections/feishu3.svg"
                                alt="feishu" />
                        </div>
                        <p className="flex-1 text-sm">
                            <span className='font-medium'>
                                <FormattedMessage id="settings.account.binding.thirepartyaccount.lark" />
                            </span>
                        </p>
                        <div className="flex gap-x-2">
                            <LinkButton />
                        </div>
                    </div>
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 bg-slate-100 rounded">
                            <img className="w-full h-full"
                                src="https://files.authing.co/authing-console/social-connections/dingding2.svg"
                                alt="dingtalk" />
                        </div>
                        <p className="flex-1 text-sm">
                            <span className='font-medium'>
                                <FormattedMessage id="settings.account.binding.thirepartyaccount.dingtalk" />
                            </span>
                            <span className="text-blue-600 text-sm before:content-[':'] before:mx-1 before:text-slate-600">
                                张三
                            </span>
                        </p>
                        <div className="flex gap-x-2">
                            <UnLinkButton />
                        </div>
                    </div>
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 bg-slate-100 rounded">
                            <img className="w-full h-full"
                                src="https://files.authing.co/authing-console/social-connections/wechatIdentitySource.svg"
                                alt="wechat" />
                        </div>
                        <p className="flex-1 text-sm">
                            <span className='font-medium'>
                                <FormattedMessage id="settings.account.binding.thirepartyaccount.wechat" />
                            </span>
                            <span className="text-blue-600 text-sm before:content-[':'] before:mx-1 before:text-slate-600">
                                张三
                            </span>
                        </p>
                        <div className="flex gap-x-2">
                            <UnLinkButton />
                        </div>
                    </div>
                </div>
            </ContentBlock>
        </div>
    )
}

export default AccountSettingsPage