import {CurrentUserInfo} from '@/@types/auth';
import React from 'react';
import {Link, history, useModel, useDispatch} from 'umi';
import {Button} from '@/components/ui/button';
import {DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger} from '@/components/ui/dropdown-menu';
import {Copyright, LogOut, Server, User, UserSquare} from 'lucide-react';


export interface HomePageProps {
    isAuthenticated: boolean
    currentUser?: CurrentUserInfo
}

const HomePage: React.FC<HomePageProps> = (props: HomePageProps) => {
    const {initialState} = useModel('@@initialState')
    const {currentUser} = initialState ?? {}
    const dispatch = useDispatch()
    
    const handleLogOut = ()=>{
        dispatch({
            type: "login/logout"
        })
    }

    return (
        <div className="w-full">
            <header className="py-4 fixed top-0 w-full px-4 sm:px-6 lg:px-8">
                <div className="container mx-auto">
                    <nav className="relative z-50 flex justify-between">
                        <div className="flex items-center md:gap-x-12">
                            <Link aria-label="Home" to="/#">
                                <h1 className="text-center text-lg font-bold tracking-wide text-blue-600 dark:text-gray-100">
                                    OpenAuthing
                                </h1>
                            </Link>
                            <div className="hidden md:flex md:gap-x-6">
                                <Link className="inline-block rounded-lg px-2 py-1 text-sm text-slate-700 hover:bg-slate-100 hover:text-slate-900"
                                      to="/#features">
                                    Features
                                </Link>
                                <Link className="inline-block rounded-lg px-2 py-1 text-sm text-slate-700 hover:bg-slate-100 hover:text-slate-900"
                                      to="/#testimonials">Testimonials</Link>
                                <Link className="inline-block rounded-lg px-2 py-1 text-sm text-slate-700 hover:bg-slate-100 hover:text-slate-900"
                                      to="/#pricing">Pricing</Link>
                            </div>
                        </div>
                        <div className="flex items-center gap-x-5 md:gap-x-8">
                            <div className="hidden md:block">
                                {currentUser ?
                                    <DropdownMenu>
                                        <DropdownMenuTrigger asChild={true}>
                                            <Button variant="link">{currentUser.nickname}</Button>
                                        </DropdownMenuTrigger>
                                        <DropdownMenuContent align="end" className="text-sm text-gray-600 p-2 w-32">
                                            <DropdownMenuGroup>
                                                <DropdownMenuItem>
                                                    <Link to="/settings/profile"
                                                          className="flex items-center gap-x-2">
                                                        <UserSquare className="w-4 h-4"/>
                                                        <span>个人信息</span>
                                                    </Link>
                                                </DropdownMenuItem>
                                            </DropdownMenuGroup>
                                            <DropdownMenuSeparator/>
                                            <DropdownMenuGroup>
                                                <DropdownMenuItem className="flex items-center gap-x-2">
                                                    <div onClick={handleLogOut}
                                                         className="flex items-center gap-x-2">
                                                        <LogOut className="w-4 h-4"/>
                                                        <span>退出登录</span>
                                                    </div>
                                                </DropdownMenuItem>
                                            </DropdownMenuGroup>
                                        </DropdownMenuContent>
                                    </DropdownMenu> :
                                    <Button variant="link" onClick={() => history.push('/account/login')}>登录</Button>
                                }

                            </div>
                            <div className="-mr-1 md:hidden">
                                <div data-headlessui-state="">
                                    <button
                                        className="relative z-10 flex h-8 w-8 items-center justify-center [&amp;:not(:focus-visible)]:focus:outline-none"
                                        aria-label="Toggle Navigation" type="button" aria-expanded="false" data-headlessui-state=""
                                        id="headlessui-popover-button-:R3p6:">
                                        <svg aria-hidden="true" className="h-3.5 w-3.5 overflow-visible stroke-slate-700" fill="none" strokeWidth="2"
                                             strokeLinecap="round">
                                            <path d="M0 1H14M0 7H14M0 13H14" className="origin-center transition"></path>
                                            <path d="M2 2L12 12M12 2L2 12" className="origin-center transition scale-90 opacity-0"></path>
                                        </svg>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </nav>
                </div>
            </header>
            <main>
                <div className="h-screen w-full bg-slate-200">

                </div>

                <div className="h-screen w-full bg-red-50">

                </div>
            </main>

            <footer className="border-t py-8">
                <div className="mx-auto container">
                    <p className="flex items-center justify-center text-sm text-gray-400 gap-x-2">
                        OpenAuthing<Copyright className="w-4 h-4"/>BeniceSoft
                    </p>
                </div>
            </footer>
        </div>
    );
}


export default HomePage