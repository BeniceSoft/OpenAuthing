import ContentBlock from '@/components/ContentBlock'
import { Edit3Icon, LinkIcon, MailIcon, PhoneIcon, UnlinkIcon } from 'lucide-react'
import React from 'react'

const LinkButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button type="button"
        ref={ref}
        className="flex items-center gap-x-1 text-sm text-blue-600 hover:text-blue-700 transition-colors">
        <LinkIcon className="w-4 h-4" />
        <span>绑定</span>
    </button>
))

const EditButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button className="flex items-center gap-x-1 text-sm text-blue-600 hover:text-blue-700 transition-colors">
        <Edit3Icon className="w-4 h-4" />
        <span>修改</span>
    </button>
))

const UnLinkButton = React.forwardRef<HTMLButtonElement, any>(({ }, ref) => (
    <button className="flex items-center gap-x-1 text-sm">
        <UnlinkIcon className="text-orange-600 w-4 h-4" />
        <span className="text-gray-600 hover:text-gray-800 transition-colors">解除绑定</span>
    </button>
))

const AccountSettingsPage: React.FC = () => {
    return (
        <div className="">
            <ContentBlock title="邮箱和手机号">
                <div className="flex flex-col mb-8 gap-y-6">
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 p-2 bg-slate-100 rounded-md">
                            <MailIcon className="w-5 h-5 stroke-slate-600" />
                        </div>
                        <p className="flex-1 text-sm">邮箱</p>
                        <div className="flex gap-x-2">
                            <LinkButton />
                        </div>
                    </div>
                    <div className="flex items-center justify-center gap-x-4">
                        <div className="w-9 h-9 p-2 bg-slate-100 rounded-md">
                            <PhoneIcon className="w-5 h-5 stroke-slate-600" />
                        </div>
                        <p className="flex-1 text-sm">
                            手机号
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
            <ContentBlock title="第三方账号">
                <div className="flex flex-col gap-y-6">
                    <div className="flex items-center gap-x-4">
                        <div className="w-9 h-9 bg-slate-100 rounded">
                            <img className="w-full h-full"
                                src="https://files.authing.co/authing-console/social-connections/feishu3.svg"
                                alt="feishu" />
                        </div>
                        <p className="flex-1 text-sm">
                            飞书
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
                            钉钉
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
                            微信
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