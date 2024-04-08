import AvatarCorpDialog from "@/components/AvatarCorpDialog";
import LangSelect from "@/components/LangSelect";
import Logo from "@/components/Logo";
import ThemeSwitch from "@/components/ThemeSwitch";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import withAuth from "@/hocs/withAuth";
import AccountService from "@/services/account.service";
import { CameraIcon, FingerprintIcon, FootprintsIcon, PowerIcon, UserIcon, VenetianMaskIcon } from "lucide-react";
import React, { useEffect } from "react";
import { useState } from "react";
import { FormattedMessage, Link, Outlet, useDispatch, useLocation, useModel } from "umi";

interface NavMenuItemProps {
    selected?: boolean
    icon: React.ReactNode
    text: React.ReactNode
    link: string
}

const NavMenuItem = React.forwardRef<HTMLAnchorElement, NavMenuItemProps>(({
    selected = false,
    link,
    icon,
    text
}: NavMenuItemProps, ref: React.ForwardedRef<HTMLAnchorElement>) => {
    return (
        <Link ref={ref} to={link}
            className="px-2 py-2.5 rounded flex gap-x-2 items-center relative font-medium hover:bg-blue-50 hover:text-blue-600 aria-selected:bg-blue-50 aria-selected:text-blue-600 aria-selected:before:content-[''] aria-selected:before:h-[20px] before:w-[4px] aria-selected:before:bg-blue-600 before:hidden aria-selected:before:block aria-selected:before:absolute before:left-[-6px] before:rounded"
            aria-selected={selected}>
            {icon}
            <span>{text}</span>
        </Link>
    )
})

interface HeaderProps {
    avatar?: string
    onLogOut?: () => void
}

const Header = ({
    avatar, onLogOut
}: HeaderProps) => {

    return (
        <div className="w-full h-[64px] border-b bg-white text-black flex items-center justify-between px-4 lg:px-8">
            <Link className="text-base font-bold tracking-wide text-blue-600 dark:text-gray-100"
                to="/settings">
                <Logo className="w-40" />
            </Link>
            <div className="flex items-center text-gray-600 hover:text-gray-800 transition-colors gap-x-6 text-sm">
                <LangSelect />
                <ThemeSwitch />
                <DropdownMenu>
                    <DropdownMenuTrigger asChild={true}>
                        <Avatar className="w-8 h-8 cursor-pointer">
                            <AvatarImage src={avatar}
                                alt="avatar" />
                            <AvatarFallback>
                                <img src="https://files.authing.co/authing-console/default-user-avatar.png" />
                            </AvatarFallback>
                        </Avatar>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        <DropdownMenuItem className="flex gap-x-2 text-xs items-center" onClick={onLogOut}>
                            <PowerIcon className="w-3 h-3" />
                            <FormattedMessage id="common.signout" />
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            </div>
        </div >
    )
}

const SettingsLayout = function (props: any) {
    const { initialState, refresh } = useModel('@@initialState')
    useEffect(() => {
        refresh()
    }, [])

    const dispatch = useDispatch()
    const [avatarCorpDialogOpened, setAvatarCorpDialogOpened] = useState<boolean>()
    const [avatarSrc, setAvatarSrc] = useState<string>()

    const fileInputRef = React.useRef<any>()

    const { pathname } = useLocation()
    const normalizedPathname = pathname.toLocaleLowerCase()

    const isSelected = (key: string): boolean => {
        return normalizedPathname === key
    }

    const onAvatarButtonClick = () => {
        fileInputRef.current?.click();
    }

    const onSelectAvatarFile = (e: React.ChangeEvent<HTMLInputElement>) => {
        console.log('onSelectAvatarFile')
        if (e.target.files && e.target.files.length > 0) {
            const reader = new FileReader()
            reader.addEventListener('load', () => {
                console.log('lalalalala')
                setAvatarSrc(reader.result?.toString() || '')
                setAvatarCorpDialogOpened(true)
            },)
            reader.readAsDataURL(e.target.files[0])
        }
    };

    const onAvatarCorpDialogOpenChanged = (open: boolean) => {
        fileInputRef.current.value = ''
        setAvatarSrc(undefined);
        setAvatarCorpDialogOpened(open)
    }

    const handleUploadAvatar = async (croppedBlob: Blob | null) => {
        if (croppedBlob) {
            await AccountService.uploadAvatar(croppedBlob)
        }
    }

    const handleLogOut = () => {
        dispatch({
            type: "login/logout"
        })
    }

    if (normalizedPathname.startsWith('/settings/2fa')) {
        return (
            <div className="w-screen">
                <Header avatar={initialState?.currentUser?.avatar} onLogOut={handleLogOut} />
                <div className="flex-1 overflow-auto pb-6">
                    <main className="lg:container mx-4 lg:mx-auto">
                        <Outlet />
                    </main>
                </div>
            </div>
        )
    }

    return (
        <div className="w-screen min-w-[720px] h-screen flex flex-col overflow-hidden bg-gray-50">
            <Header avatar={initialState?.currentUser?.avatar} onLogOut={handleLogOut} />
            <div className="flex-1 overflow-auto pb-8">
                <main className="lg:container mx-4 lg:mx-auto space-y-4 lg:space-y-0 lg:flex justinfy-between pt-4 gap-x-4 items-start">
                    <nav className="min-w-72 lg:w-80 text-sm gap-y-2 lg:gap-y-4 grid">
                        <div className="w-full px-4 py-8 bg-white rounded-lg shadow-sm flex flex-col items-center justify-center gap-y-2">
                            <div className="w-28 h-28 border-gray-400 rounded-full relative">
                                <Avatar className="w-full h-full">
                                    <AvatarImage src={initialState?.currentUser?.avatar} />
                                    <AvatarFallback>
                                        <img className="w-full h-full"
                                            src="https://files.authing.co/authing-console/default-user-avatar.png" />
                                    </AvatarFallback>
                                </Avatar>
                                <button type="button"
                                    onClick={onAvatarButtonClick}
                                    className="w-8 h-8 bg-primary text-primary-foreground rounded-full border-[2px] border-white absolute bottom-1 right-1 inline-flex items-center justify-center">
                                    <CameraIcon className="w-4 h-4" />
                                </button>
                                <input type="file" ref={fileInputRef}
                                    className="hidden" accept="image/*"
                                    onChangeCapture={onSelectAvatarFile} />
                            </div>
                            <h2 className="text-gray-800 text-xl">{initialState?.currentUser?.nickname}</h2>
                        </div>
                        <div className="bg-white p-6 rounded-lg shadow-sm">
                            <div className="gap-y-1.5 grid">
                                <NavMenuItem link="/settings/profile"
                                    selected={isSelected('/settings/profile')}
                                    icon={<UserIcon className="w-4 h-4" />}
                                    text={<FormattedMessage id="settings.menu.profile" />} />
                                <NavMenuItem link="/settings/account"
                                    selected={isSelected('/settings/account')}
                                    icon={<VenetianMaskIcon className="w-4 h-4" />}
                                    text={<FormattedMessage id="settings.menu.account" />} />
                            </div>
                            <div className="w-full my-1.5 h-[1px] bg-slate-200" />
                            <div className="gap-y-1.5 grid">
                                <NavMenuItem link="/settings/security"
                                    selected={isSelected('/settings/security')}
                                    icon={<FingerprintIcon className="w-4 h-4" />}
                                    text={<FormattedMessage id="settings.menu.security" />} />
                                <NavMenuItem link="/settings/login-logs"
                                    selected={isSelected('/settings/login-logs')}
                                    icon={<FootprintsIcon className="w-4 h-4" />}
                                    text={<FormattedMessage id="settings.menu.loginlogs" />} />
                            </div>
                        </div>
                    </nav>
                    <div className="flex-1 bg-white p-6 rounded-lg shadow-sm">
                        <Outlet />
                    </div>
                </main>
            </div>
            <AvatarCorpDialog open={avatarCorpDialogOpened}
                onOpenChange={onAvatarCorpDialogOpenChanged}
                imageSrc={avatarSrc}
                onConfirm={handleUploadAvatar} />
        </div>
    )
}

export default withAuth(SettingsLayout)