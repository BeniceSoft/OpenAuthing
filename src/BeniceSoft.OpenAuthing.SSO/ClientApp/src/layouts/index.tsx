import { Outlet, useLocation } from "umi"
import { Toaster } from "react-hot-toast";
import { useEffect } from "react";
import { syncTheme } from "@/lib/misc";

export default () => {

    const { pathname } = useLocation();

    if (process.env.NODE_ENV === 'development') {
        if (pathname === '/connect/authorize') {
            console.log(window.location)
            const href = window.location.href
            // @ts-ignore
            window.location = href.replace('8000', '5129')
        }
    }

    useEffect(() => {
        syncTheme()
    }, [pathname]);

    return (
        <div>
            <Outlet />
            <Toaster />
        </div>
    )
}