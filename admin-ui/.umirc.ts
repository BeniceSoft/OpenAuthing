import { defineConfig } from "umi";

export default defineConfig({
    title: 'OpenAuthing',
    // mock: false,
    esbuildMinifyIIFE: true,
    proxy: {
        '/api': {
            'target': 'http://localhost:5129/',
            'changeOrigin': true
        },
        '/uploadFiles': {
            'target': 'http://localhost:5129/',
            'changeOrigin': true
        },
    },
    routes: [
        {
            path: "/",
            routes: [{
                path: '/admin',
                component: '@/layouts/admin',
                routes: [
                    { path: '/admin/dashboard', component: 'admin/dashboard' },
                    { path: '/admin/clients', component: 'admin/clients' },
                    { path: '/admin/clients/:id', component: 'admin/clients/detail' },

                    { path: '/admin/idps', component: 'admin/idps' },
                    { path: '/admin/idps/providers', component: 'admin/idps/providers' },
                    { path: '/admin/idps/create/:providerName', component: 'admin/idps/create' },

                    { path: '/admin/branding', component: 'admin/branding' },
                    { path: '/admin/setting', component: 'admin/setting' },

                    { path: '/admin/org/departments', component: 'admin/departments' },

                    { path: '/admin/org/users', component: 'admin/users' },
                    { path: '/admin/org/users/:id', component: 'admin/users/detail' },

                    { path: '/admin/org/user-groups', component: 'admin/user-groups' },
                    { path: '/admin/org/user-groups/create', component: 'admin/user-groups/create' },
                    { path: '/admin/org/user-groups/detail/:id', component: 'admin/user-groups/detail' },

                    { path: '/admin/permission/spaces', component: 'admin/permission/spaces' },
                    { path: '/admin/permission/spaces/create', component: 'admin/permission/spaces/create' },
                    { path: '/admin/permission/spaces/detail/:id', component: 'admin/permission/spaces/detail' },

                    { path: '/admin/permission/roles', component: 'admin/permission/roles' },
                    { path: '/admin/permission/roles/create', component: 'admin/permission/roles/create' },
                    { path: '/admin/permission/roles/detail/:id', component: 'admin/permission/roles/detail' },

                    { path: '/admin/permission/general', component: 'admin/permission/general' },

                    { path: '/admin/permission/data', component: 'admin/permission/data' },
                    { path: '/admin/permission/data/auth/create', component: 'admin/permission/data/auth/create' },
                    { path: '/admin/permission/data/resources/create', component: 'admin/permission/data/resources/create' },
                    { path: '/admin/permission/data/resources/detail/:id', component: 'admin/permission/data/resources/detail' },

                    { path: '/admin', redirect: '/admin/dashboard' }
                ]
            }, {
                path: '/',
                redirect: '/admin/dashboard'
            }]
        }, {
            path: '/*',
            component: '404'
        }
    ],
    npmClient: "npm",
    tailwindcss: {},
    plugins: [
        '@umijs/plugins/dist/initial-state',
        '@umijs/plugins/dist/model',
        "@umijs/plugins/dist/tailwindcss",
        "@umijs/plugins/dist/dva",
        "@umijs/plugins/dist/request",
        "@umijs/plugins/dist/qiankun",
    ],
    dva: {},
    request: {
        dataField: 'data'
    },
    clientLoader: {},
    model: {},
    initialState: {},
    qiankun: {
        slave: {
            // enable: false
        },
    },
});
