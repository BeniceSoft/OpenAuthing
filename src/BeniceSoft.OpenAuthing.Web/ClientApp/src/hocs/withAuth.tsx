import { Navigate, useLocation, useModel } from 'umi'

const withAuth = (Component: any) => () => {
    const { initialState } = useModel('@@initialState');

    console.log('initialState', initialState)
    
    if (!(initialState?.isAuthenticated ?? false)) {
        const { pathname } = useLocation()
        return <Navigate to={`/account/login?returnUrl=${encodeURIComponent(pathname)}`} />;
    }

    return <Component />;
}

export default withAuth