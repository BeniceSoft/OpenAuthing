import { defineConfig } from "umi";

export default defineConfig({
    define: {
        SHOW_OIDC_LOGGING:false,
        
        AM_ODIC_AUTHORITY: 'http://localhost:5129',
        AM_ODIC_CLIENT_ID: 'linkmore-ka-am-admin',
        AM_ODIC_CLIENT_SECRET: '',

        AM_USER_PROFILE_URL: 'http://localhost:5129/#/settings/profile',

        AM_ADMIN_API_BASE_URL: 'http://localhost:5129'
    }
})