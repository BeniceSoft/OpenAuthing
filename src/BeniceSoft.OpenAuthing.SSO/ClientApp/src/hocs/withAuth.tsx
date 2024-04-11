import { Navigate, useLocation, useModel, useRequest } from 'umi'
import AuthService from "@/services/auth.service";

const withAuth = (Component: any) => () => {

    const { initialState: { isAuthenticated } } = useModel("@@initialState")

    console.log(isAuthenticated)

    if (!isAuthenticated) {
        const { pathname } = useLocation()
        return <Navigate to={`/account/login?returnUrl=${encodeURIComponent(pathname)}`} />;
    }

    return <Component />;
}

export default withAuth