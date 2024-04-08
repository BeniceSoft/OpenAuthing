import { defineConfig } from "umi";

export default defineConfig({
    title: 'OpenAuthing',
    mock: false,
    esbuildMinifyIIFE: true,
    proxy: {
        '/api': {
            'target': 'http://127.0.0.1:5129/',
            'changeOrigin': true,
            'pathRewrite': { '^/api': '/api' },
        },
        '/uploadFiles': {
            'target': 'http://127.0.0.1:5129/',
            'changeOrigin': true
        },
        '/connect': {
            'target': 'http://127.0.0.1:5129/',
            'changeOrigin': true
        },
    },
    history: {
        type: 'hash'
    },
    routes: [
        {
            path: "/",
            component: "@/layouts/index",
            routes: [{
                path: '/account',
                component: '@/layouts/account',
                routes: [
                    { path: '/account/login', component: 'account/login' },
                    { path: '/account/loginwith2fa', component: 'account/loginwith2fa' },
                    { path: '/account/loginwithrecoverycode', component: 'account/loginwithrecoverycode' }
                ]
            }, {
                path: '/settings',
                component: '@/layouts/settings',
                routes: [
                    { path: '/settings', redirect: '/settings/profile' },
                    { path: '/settings/profile', component: 'settings/profile' },
                    { path: '/settings/account', component: 'settings/account' },
                    { path: '/settings/security', component: 'settings/security' },
                    { path: '/settings/login-logs', component: 'settings/loginlogs' },
                    {
                        path: '/settings/2fa',
                        routes: [
                            { path: '/settings/2fa/enable-authenticator', component: 'settings/2fa/enableauthenticator' },
                            { path: '/settings/2fa/show-recovery-codes', component: 'settings/2fa/showrecoverycodes' },
                            { path: '/settings/2fa/recovery-codes', component: 'settings/2fa/recoverycodes' },
                        ]
                    }
                ]
            }, {
                path: '/',
                component: 'index'
            }]
        }, {
            path: '/*',
            component: '404'
        }
    ],
    scripts: [
        "./node_modules/preline/preline.js"
    ],
    links: [
        { href: "https://rsms.me/inter/inter.css", rel: "stylesheet" }
    ],
    npmClient: "npm",
    tailwindcss: {},
    plugins: [
        '@umijs/plugins/dist/initial-state',
        '@umijs/plugins/dist/model',
        "@umijs/plugins/dist/tailwindcss",
        "@umijs/plugins/dist/dva",
        "@umijs/plugins/dist/request",
        '@umijs/plugins/dist/locale'
    ],
    locale: {
        default: 'zh-CN',
        baseSeparator: '-'
    },
    dva: {},
    request: {
        dataField: 'data'
    },
    clientLoader: {},
    model: {},
    initialState: {}
});
