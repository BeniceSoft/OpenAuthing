import Spin from '@/components/Spin';
import Authenticating from '@/components/oidc/Authenticating';
import AuthenticatingError from '@/components/oidc/AuthenticatingError';
import CallBackSuccess from '@/components/oidc/CallBackSuccess';
import OidcProvider from '@/components/oidc/OidcProvider';
import SessionLostCompoent from '@/components/oidc/SessionLost';
import { OidcConfiguration, TokenRenewMode } from '@axa-fr/react-oidc';
import { VanillaOidc } from '@axa-fr/react-oidc/dist/vanilla/vanillaOidc';
import { Toaster, toast } from 'react-hot-toast';
import { Outlet } from 'umi';

const configuration: OidcConfiguration = {
  authority: AM_ODIC_AUTHORITY,
  client_id: AM_ODIC_CLIENT_ID,
  redirect_uri: window.location.origin + '/am/admin/login',
  silent_redirect_uri: window.location.origin + '/am/admin/silent-login',
  // silent_redirect_uri: window.location.origin + '/authentication/silent-callback', // Optional activate silent-signin that use cookies between OIDC server and client javascript to restore the session
  scope: 'openid profile phone roles',
  // service_worker_relative_url: '/OidcServiceWorker.js',
  service_worker_only: false,
  service_worker_convert_all_requests_to_cors: true,
  storage: localStorage,
  token_renew_mode: TokenRenewMode.access_token_invalid,
};

export default function Layout() {

  const handleOidcEvent = (configName: string, eventName: string, data: any) => {

    SHOW_OIDC_LOGGING && console.log(`oidc:${configName}:${eventName}: ${JSON.stringify(data)}`)

    if (eventName === VanillaOidc.eventNames.silentLoginAsync_end) {
      toast.success('访问令牌刷新成功')
    }
  }

  return (
    <OidcProvider
      configuration={configuration}
      loadingComponent={Spin}
      authenticatingComponent={Authenticating}
      authenticatingErrorComponent={AuthenticatingError}
      callbackSuccessComponent={CallBackSuccess}
      sessionLostComponent={SessionLostCompoent}
      onEvent={handleOidcEvent}>
      <Toaster />
      <Outlet />
    </OidcProvider>
  );
}
