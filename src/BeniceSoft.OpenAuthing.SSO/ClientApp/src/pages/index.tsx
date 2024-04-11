import { CurrentUserInfo } from '@/@types/auth';
import React, { useEffect } from 'react';
import { FormattedMessage, Link, history, useModel } from 'umi';
import { Button } from '@/components/ui/button';
import { Copyright, LogOut, UserSquare } from 'lucide-react';
import Logo from '@/components/Logo';
import { HSDropdown } from 'preline/preline'
import { Avatar } from '@/components/ui/avatar';


export interface HomePageProps {
    isAuthenticated: boolean
    currentUser?: CurrentUserInfo
}

const HomePage: React.FC<HomePageProps> = (props: HomePageProps) => {
    const { initialState } = useModel('@@initialState')
    const { currentUser } = initialState ?? {}

    useEffect(() => {
        HSDropdown.autoInit()
    }, [])

    return (
        <div className="w-full">
            <header className="py-4 fixed top-0 w-full px-4 sm:px-6 lg:px-8">
                <div className="container mx-auto">
                    <nav className="relative z-50 flex justify-between">
                        <div className="flex items-center md:gap-x-12">
                            <Link aria-label="Home" to="/#">
                                <Logo className="w-44" />
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
                        <div className="flex items-center gap-x-5 md:gap-x-8 [--placement:bottom-right]">
                            <div className="hidden md:block">
                                {currentUser ?
                                    <div className="hs-dropdown relative inline-flex">
                                        <Button variant="link" id="hs-dropdown-currentuser" type="button"
                                            className="hs-dropdown-toggle inline-flex justify-center items-center gap-x-2 text-sm">
                                            {/* <img className="inline-block size-8 rounded-full" src={currentUser.avatar} alt={currentUser.userName}></img> */}
                                            <Avatar src="currentUser.avatar"
                                                size="xs"
                                                fallback="https://files.authing.co/authing-console/default-user-avatar.png"
                                                alt="avatar" />
                                            {currentUser.nickname}
                                        </Button>

                                        <div className="hs-dropdown-menu text-sm font-medium transition-[opacity,margin] space-y-1 duration hs-dropdown-open:opacity-100 opacity-0 w-44 hidden z-10 mt-1 min-w-40 bg-white shadow-md rounded-lg p-2 dark:bg-gray-800 dark:border dark:border-gray-700 dark:divide-gray-700"
                                            aria-labelledby="hs-dropdown-currentuser">
                                            <Link to="/settings"
                                                className="flex items-center gap-x-3.5 py-2 px-3 rounded-lg text-sm text-gray-800 hover:bg-gray-100 focus:outline-none focus:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-gray-300 dark:focus:bg-gray-700">
                                                <UserSquare className="w-4 h-4" />
                                                <span className="truncate">
                                                    <FormattedMessage id="home.link.profile" />
                                                </span>
                                            </Link>
                                            <Link to="/logout"
                                                replace={true}
                                                className="flex items-center gap-x-3.5 py-2 px-3 rounded-lg text-sm text-gray-800 hover:bg-gray-100 focus:outline-none focus:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-gray-300 dark:focus:bg-gray-700">
                                                <LogOut className="w-4 h-4" />
                                                <span className="truncate">
                                                    <FormattedMessage id="home.link.signout" />
                                                </span>
                                            </Link>
                                        </div>
                                    </div> :
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
                        OpenAuthing<Copyright className="w-4 h-4" />BeniceSoft
                    </p>
                </div>
            </footer>
        </div>
    );
}


export default HomePage