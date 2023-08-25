import { getOidc } from "@/lib/utils";
import { OidcUserStatus, StringMap } from "@axa-fr/react-oidc";
import { OidcAccessToken, OidcIdToken } from "@axa-fr/react-oidc/dist/ReactOidc";
import { OidcUserInfo, VanillaOidc } from "@axa-fr/react-oidc/dist/vanilla/vanillaOidc";
import { FC, PropsWithChildren, useEffect, useState } from "react";

export type OidcSecureProps = {
    callbackPath?: string;
    extras?: StringMap;
    configurationName?: string;
};

export const OidcSecure: FC<PropsWithChildren<OidcSecureProps>> = ({ children, callbackPath, extras, configurationName = 'default' }) => {
    const getOidc = window.__POWERED_BY_QIANKUN__ ? window.qiankun.getOidc : VanillaOidc.get;
    const oidc = getOidc(configurationName);
    useEffect(() => {
        if (!oidc.tokens) {
            oidc.loginAsync(callbackPath, extras);
        }
    }, [configurationName, callbackPath, extras]);

    if (!oidc.tokens) {
        return null;
    }
    return <>{children}</>;
};

export const withOidcSecure = (
    WrappedComponent: FC<PropsWithChildren<OidcSecureProps>>,
    callbackPath = undefined,
    extras = undefined,
    configurationName = 'default',
) => (props: any) => {
    return (
        <OidcSecure callbackPath={callbackPath} extras={extras} configurationName={configurationName}>
            <WrappedComponent {...props} />
        </OidcSecure>
    );
};


export const useOidcUser = <T extends OidcUserInfo = OidcUserInfo>(configurationName = 'default') => {
    //@ts-ignore
    const [oidcUser, setOidcUser] = useState<OidcUser<T>>({ user: null, status: OidcUserStatus.Unauthenticated });

    const oidc = getOidc(configurationName);
    useEffect(() => {
        let isMounted = true;
        if (oidc && oidc.tokens) {
            setOidcUser({ ...oidcUser, status: OidcUserStatus.Loading });
            oidc.userInfoAsync()
                .then((info) => {
                    if (isMounted) {
                        // @ts-ignore
                        setOidcUser({ user: info, status: OidcUserStatus.Loaded });
                    }
                })
                .catch(() => setOidcUser({ ...oidcUser, status: OidcUserStatus.LoadingError }));
        }
        return () => { isMounted = false; };
    }, []);

    return { oidcUser: oidcUser.user, oidcUserLoadingState: oidcUser.status };
};

type GetOidcFn = {
    (configurationName?: string): any;
}

const defaultIsAuthenticated = (getOidc: GetOidcFn, configurationName: string) => {
    let isAuthenticated = false;
    const oidc = getOidc(configurationName);
    if (oidc) {
        isAuthenticated = getOidc(configurationName).tokens != null;
    }
    return isAuthenticated;
};

export const useOidc = (configurationName = 'default') => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(defaultIsAuthenticated(getOidc, configurationName));

    useEffect(() => {
        let isMounted = true;
        const oidc = getOidc(configurationName);
        setIsAuthenticated(defaultIsAuthenticated(getOidc, configurationName));
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        const newSubscriptionId = oidc.subscribeEvents((name: string, data: any) => {
            if (name === VanillaOidc.eventNames.logout_from_another_tab || name === VanillaOidc.eventNames.logout_from_same_tab || name === VanillaOidc.eventNames.token_aquired) {
                if (isMounted) {
                    setIsAuthenticated(defaultIsAuthenticated(getOidc, configurationName));
                }
            }
        });
        return () => {
            isMounted = false;
            oidc.removeEventSubscription(newSubscriptionId);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [configurationName]);

    const login = (callbackPath: string | undefined = undefined, extras: StringMap | undefined = undefined, silentLoginOnly = false) => {
        return getOidc(configurationName).loginAsync(callbackPath, extras, false, undefined, silentLoginOnly);
    };
    const logout = (callbackPath: string | null | undefined = undefined, extras: StringMap | undefined = undefined) => {
        return getOidc(configurationName).logoutAsync(callbackPath, extras);
    };
    const renewTokens = async (extras: StringMap | undefined = undefined): Promise<OidcAccessToken | OidcIdToken> => {
        const tokens = await getOidc(configurationName).renewTokensAsync(extras);

        return {
            // @ts-ignore
            accessToken: tokens.accessToken,
            // @ts-ignore
            accessTokenPayload: tokens.accessTokenPayload,
            // @ts-ignore
            idToken: tokens.idToken,
            // @ts-ignore
            idTokenPayload: tokens.idTokenPayload,
        };
    };
    return { login, logout, renewTokens, isAuthenticated };
};