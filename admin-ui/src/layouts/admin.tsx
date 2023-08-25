import { Switch, Transition } from "@headlessui/react";
import { Link, Outlet, useLocation, useModel } from "umi"
import moon from '@/assets/icons/moon.png'
import sun from '@/assets/icons/sun.png'
import React, { useEffect, useState } from "react";
import classNames from "classnames";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { BookOpen, ChevronDown, ChevronUp, Key, LinkIcon, MonitorSmartphone, MoreHorizontalIcon, Network, Palette, Settings, Share2, Shirt } from "lucide-react";
import { Tooltip, TooltipArrow, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuPortal, DropdownMenuSub, DropdownMenuSubContent, DropdownMenuSubTrigger, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { PowerIcon } from "lucide-react";
import { LinkProps } from "react-router-dom";
import { withOidcSecure, useOidc, useOidcUser } from "@/components/oidc/OidcSecure";

const AdminHeader = () => {
    const { initialState, loading, error, refresh, setInitialState } = useModel('@@initialState');
    const notDark = initialState?.theme !== 'dark'

    console.log(initialState)

    const onChangeTheme = () => {
        const html = document.querySelector('html');
        const currentTheme = html!.classList.toggle('dark') ? 'dark' : 'light';
        localStorage.setItem('theme', currentTheme)
        setInitialState({
            ...initialState,
            theme: currentTheme
        })
    }

    return (
        <div className="w-full flex justify-between z-[100] px-5 py-2">
            <div></div>
            <div className="flex items-center gap-x-4 text-sm">
                <div>
                    <Switch
                        checked={notDark}
                        onChange={onChangeTheme}
                        className={`${notDark ? 'bg-gray-100' : 'bg-slate-600'
                            } relative inline-flex h-6 w-11 items-center rounded-full transition-colors focus:outline-none`}
                    >
                        <img src={notDark ? sun : moon}
                            className={`${notDark ? 'translate-x-6' : 'translate-x-1'} inline-block h-4 w-4 transform rounded-full transition-transform`} />
                    </Switch>
                </div>
                <div>
                    <Avatar className="w-8 h-8">
                        <AvatarImage src="https://files.authing.co/authing-console/default-user-avatar.png"
                            alt="avatar" />
                        <AvatarFallback>AM</AvatarFallback>
                    </Avatar>
                </div>
            </div>
        </div>
    )
}

const NestedNavMenu = ({
    icon,
    label,
    items,
    selected = false
}: { icon: React.ReactNode, label: string, selected?: boolean, items: Array<{ label: string, href: string }> }) => {
    const [opened, setOpended] = useState(selected)

    return (
        <div className="cursor-pointer">
            <div className="group flex items-center px-2 py-2 rounded hover:bg-blue-50 transition-colors duration-300"
                aria-selected={selected}
                onClick={() => setOpended(!opened)}>
                <span>{icon}</span>
                <span className="flex-1 ml-3 whitespace-nowrap group-hover:text-blue-600 group-aria-selected:text-blue-600 transition-colors duration-300">
                    {label}
                </span>
                <span className="group-hover:text-blue-600">
                    {opened ?
                        <ChevronUp className="w-4 h-4 stroke-2" /> :
                        <ChevronDown className="w-4 h-4 stroke-2" />
                    }
                </span>
            </div>

            <Transition
                as="div"
                className={classNames("grid transition-all", opened ? "grid-rows-[1fr]" : "grid-rows-[0fr]")}
                show={opened}
                unmount={false}
                enterFrom="grid-rows-[0fr]"
                enterTo="grid-rows-[1fr]"
                leaveFrom="grid-rows-[1fr]"
                leaveTo="grid-rows-[0fr]">
                <div className="w-full overflow-hidden">
                    {items.map(item => (
                        <Link to={item.href}
                            key={item.href}
                            data-key={item.href}
                            className="am-nav-item mt-2 block py-2 pl-10 whitespace-nowrap rounded text-gray-600 hover:bg-blue-50 hover:text-blue-600 aria-selected:text-blue-600 aria-selected:bg-blue-100 transition-colors duration-300">
                            {item.label}
                        </Link>
                    ))}
                </div>
            </Transition>
        </div>
    );
}

interface NavMenuItemProps extends LinkProps {
    datakey: string
    text: React.ReactNode
    icon?: React.ReactNode
}
const NavMenuItem = ({
    to,
    datakey,
    text,
    icon
}: NavMenuItemProps) => {
    return (
        <Link to={to}
            data-key={datakey}
            className="am-nav-item group flex items-center px-2 py-2 rounded hover:bg-blue-50 aria-selected:bg-blue-100 transition-colors duration-300">
            {icon}
            <span className="ml-3 group-hover:text-blue-600 group-aria-selected:text-blue-600 transition-colors duration-300">
                {text}
            </span>
        </Link>
    )
}

const NavMenu = () => {
    const { pathname } = useLocation()
    const currentPaths = pathname.toLocaleLowerCase().split('/')

    useEffect(() => {
        const items = document.getElementsByClassName('am-nav-item')
        for (let i = 0; i < items.length; i++) {
            const item = items[i]
            if (item.getAttribute('data-key')) {
                const itemPaths = (item.getAttribute('data-key') ?? '').split('/')
                const length = Math.min(currentPaths.length, itemPaths.length)

                if (length < 1) continue

                let selected = true
                for (var j = 0; j < length; j++) {
                    if (currentPaths[j].trim() !== itemPaths[j].trim()) {
                        selected = false
                        break
                    }
                }

                // (pathname.toLocaleLowerCase().startsWith(item.getAttribute('data-key') ?? '')).toString()
                item.ariaSelected = selected.toString()
                continue
            }
            item.ariaSelected = 'false'
        }
    }, [pathname])

    return (
        <ul className="space-y-2 text-sm text-gray-900 dark:text-white select-none">
            <li>
                <NavMenuItem to="/admin/dashboard"
                    datakey="/admin/dashboard"
                    text="数据概览"
                    icon={(
                        <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
                            <path d="M2 10a8 8 0 018-8v8h8a8 8 0 11-16 0z" className="fill-gray-400 group-hover:fill-blue-600 group-aria-selected:fill-blue-600 transition-colors duration-300"></path>
                            <path d="M12 2.252A8.014 8.014 0 0117.748 8H12V2.252z" className="fill-gray-300 group-hover:fill-gray-400 group-aria-selected:fill-gray-400 transition-colors duration-300"></path>
                        </svg>
                    )} />
            </li>
            <li>
                <NestedNavMenu
                    selected={pathname.startsWith('/admin/org')}
                    icon={(
                        <Network className="w-5 h-5 stroke-gray-500 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )}
                    label="组织机构"
                    items={[
                        { label: '组织管理', href: '/admin/org/departments' },
                        { label: '用户管理', href: '/admin/org/users' },
                        { label: '用户组管理', href: '/admin/org/user-groups' },
                    ]} />
            </li>
            <li>
                <NestedNavMenu
                    selected={pathname.startsWith('/admin/permission')}
                    icon={(
                        <Key className="w-5 h-5 stroke-gray-600 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )}
                    label="权限管理"
                    items={[
                        { label: '权限空间', href: '/admin/permission/spaces' },
                        { label: '角色管理', href: '/admin/permission/roles' },
                        { label: '常规资源权限', href: '/admin/permission/general' },
                        { label: '数据资源权限', href: '/admin/permission/data' }
                    ]} />
            </li>
            <li>
                <NavMenuItem to="/admin/clients"
                    datakey="/admin/clients"
                    text="应用管理"
                    icon={(
                        <MonitorSmartphone className="w-5 h-5 stroke-gray-600 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )} />
            </li>
            <li>
                <NavMenuItem to="/admin/idps"
                    datakey="/admin/idps"
                    text="身份提供程序"
                    icon={(
                        <Share2 className="w-5 h-5 stroke-gray-600 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )} />
            </li >
            <li>
                <NavMenuItem to="/admin/branding"
                    datakey="/admin/branding"
                    text="品牌化"
                    icon={(
                        <Palette className="w-5 h-5 stroke-gray-600 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )} />
            </li>
            <li>
                <NavMenuItem to="/admin/setting"
                    datakey="/admin/setting"
                    text="系统设置"
                    icon={(
                        <Settings className="w-5 h-5 stroke-gray-600 group-aria-selected:stroke-blue-600 group-hover:stroke-blue-600" />
                    )} />
            </li>
        </ul >
    )
}

const LeftSideFooter = () => {
    const { initialState, setInitialState } = useModel('@@initialState');
    const notDark = initialState?.theme !== 'dark'

    const onChangeTheme = () => {
        const html = document.querySelector('html');
        const currentTheme = html!.classList.toggle('dark') ? 'dark' : 'light';
        localStorage.setItem('theme', currentTheme)
        setInitialState({
            ...initialState,
            theme: currentTheme
        })
    }

    const { logout } = useOidc()
    const { oidcUser } = useOidcUser();

    return (
        <div className="p-2 h-14 flex justify-between items-center">
            <TooltipProvider>
                <Tooltip>
                    <TooltipTrigger asChild={true}>
                        <Button variant="ghost" className="w-10 h-10"
                            onClick={() => window.open(AM_USER_PROFILE_URL, '_blank')}>
                            <Avatar className="w-8 h-8">
                                <AvatarImage src={oidcUser?.picture}
                                    alt="avatar" />
                                <AvatarFallback>
                                    <img src="https://files.authing.co/authing-console/default-user-avatar.png" />
                                </AvatarFallback>
                            </Avatar>
                        </Button>
                    </TooltipTrigger>
                    <TooltipContent className="bg-black/80 text-white">
                        <span className="text-xs">{oidcUser?.nickname}</span>
                        <TooltipArrow />
                    </TooltipContent>
                </Tooltip>

                <Tooltip>
                    <TooltipTrigger asChild={true}>
                        <Button variant="ghost" className="w-10 h-10" >
                            <LinkIcon className="w-4 h-5" />
                        </Button>
                    </TooltipTrigger>
                    <TooltipContent className="bg-black/80 text-white">
                        <span className="text-xs">接口文档</span>
                        <TooltipArrow />
                    </TooltipContent>
                </Tooltip>

                <Tooltip>
                    <TooltipTrigger asChild={true}>
                        <Button variant="ghost" className="w-10 h-10" >
                            <BookOpen className="w-4 h-5" />
                        </Button>
                    </TooltipTrigger>
                    <TooltipContent className="bg-black/80 text-white">
                        <span className="text-xs">文档</span>
                        <TooltipArrow />
                    </TooltipContent>
                </Tooltip>
            </TooltipProvider>
            <DropdownMenu>
                <DropdownMenuTrigger asChild={true}>
                    <Button variant="ghost" className="w-10 h-10">
                        <MoreHorizontalIcon className="w-8 h-8" />
                    </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent className="w-56 p-2 text-xs rounded-lg text-gray-600 hover:text-gray-800 dark:text-gray-200"
                    side="right" sideOffset={16} align="end" alignOffset={4}>
                    <DropdownMenuSub>
                        <DropdownMenuSubTrigger className="flex h-8 cursor-pointer">
                            <div className="flex-1 flex gap-x-2 items-center">
                                <Shirt className="w-4 h-4" />
                                <span>外观</span>
                            </div>
                            <span>{notDark ? '浅色' : '深色'}</span>
                        </DropdownMenuSubTrigger>
                        <DropdownMenuPortal>
                            <DropdownMenuSubContent className="rounded-lg" sideOffset={12}>
                                <div className="p-4">
                                    <div className="grid grid-cols-2 gap-x-4 text-xs text-gray-500">
                                        <div className="flex flex-col gap-y-2 items-center cursor-pointer"
                                            onClick={onChangeTheme}>
                                            <img className="w-24" src="/images/light.svg" />
                                            <span className={classNames("rounded-full px-2 py-1", notDark && "bg-blue-600 text-white")}>
                                                浅色
                                            </span>
                                        </div>
                                        <div className="flex flex-col gap-y-2 items-center cursor-pointer"
                                            onClick={onChangeTheme}>
                                            <img className="w-24" src="/images/dark.svg" />
                                            <span className={classNames("rounded-full px-2 py-1", !notDark && "bg-blue-600 text-white")}>
                                                深色
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </DropdownMenuSubContent>
                        </DropdownMenuPortal>
                    </DropdownMenuSub>

                    <DropdownMenuItem className="flex gap-x-2 h-8"
                        onClick={() => logout('/logout')}>
                        <PowerIcon className="w-4 h-4" />
                        <span>退出</span>
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </div >
    )
}

const AdminLayout = () => {


    return (
        <div className="h-screen min-w-[1280px] flex bg-background text-gray-800 dark:text-white dark:bg-slate-900">
            <aside className="flex flex-col flex-initial w-[240px] min-w-[240px] h-full overflow-hidden relative bg-slate-50 dark:bg-slate-800 dark:text-white">
                <Link to="/admin">
                    <div className="px-2 py-5">
                        <h1 className="text-center text-xl font-bold tracking-wide text-blue-600 dark:text-gray-100">
                            OpenAuthing
                        </h1>
                    </div>
                </Link>
                <div className="h-full p-3 overflow-y-auto">
                    <NavMenu />
                </div>
                <LeftSideFooter />
            </aside >
            <div className="flex-auto flex flex-col relative">
                {/* <AdminHeader /> */}
                <div className="flex-1 overflow-auto xl:container xl:mx-auto p-8 py-5">
                    <Outlet />
                </div>
            </div>
        </div>
    )
}


export default withOidcSecure(AdminLayout);