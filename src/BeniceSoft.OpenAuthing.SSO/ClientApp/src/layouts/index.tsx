import { Outlet, useLocation } from "umi"
import { Toaster } from "react-hot-toast";
import { useEffect } from "react";

export default () => {

    if (process.env.NODE_ENV === 'development') {

        console.log('pathname', window.location.pathname)

        if (window.location.pathname === '/connect/authorize') {
            console.log(window.location)
            const href = window.location.href
            // @ts-ignore
            window.location = href.replace('8000', '5129')
        }
    }

    const { pathname } = useLocation();

    useEffect(() => {
        if (document.readyState === 'complete') {
            window.HSStaticMethods.autoInit('all')
        }
    }, [pathname])


    return (
        <div>
            <Outlet />
            <Toaster />
        </div>
    )
}