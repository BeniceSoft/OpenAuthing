import { Navigate, useLocation } from 'umi'
import AuthService from "@/services/auth.service";

const withAuth = (Component: any) => () => {
    const isAuthenticated = AuthService.isAuthenticated()
    
    if (!isAuthenticated) {
        const { pathname } = useLocation()
        return <Navigate to={`/account/login?returnUrl=${encodeURIComponent(pathname)}`} />;
    }

    return <Component />;
}

export default withAuth