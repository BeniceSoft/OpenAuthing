import { defineMock } from "umi";


export default defineMock({
    'GET /api/admin/idpTemplates': (req, res) => {
        setTimeout(() => {
            res.json({
                code: 200,
                data: [{
                    name: 'feishu',
                    logo: 'https://files.authing.co/authing-console/social-connections/feishu3.svg',
                    title: '飞书',
                    description: '飞书是一站式先进企业协作与管理平台。'
                }, {
                    name: 'dingtalk',
                    logo: 'https://files.authing.co/authing-console/social-connections/dingding2.svg',
                    title: '钉钉',
                    description: '钉钉是阿里巴巴出品，专为全球企业组织打造的智能移动办公平台。'
                }, {
                    name: 'oidc',
                    logo: 'https://files.authing.co/authing-console/social-connections/oidc_logo2.svg',
                    title: 'OIDC',
                    description: 'OIDC 是一个基于 OAuth2 协议的身份认证标准协议。'
                }, {
                    name: 'oauth2_0',
                    logo: 'https://files.authing.co/authing-console/social-connections/Oauth2.0.svg',
                    title: 'OAuth 2.0',
                    description: 'OAuth 2.0 是行业标准的授权协议。'
                }, {
                    name: 'github',
                    logo: 'https://files.authing.co/authing-console/social-connections/gitHub2.svg',
                    title: 'GitHub',
                    description: 'GitHub 是一个面向开源及私有软件项目的托管平台。'
                }, {
                    name: 'facebook',
                    logo: 'https://files.authing.co/authing-console/social-connections/facebook2.svg',
                    title: 'Facebook',
                    description: 'Facebook 是世界排名领先的照片分享站点。'
                }, {
                    name: 'twitter',
                    logo: 'https://files.authing.co/authing-console/social-connections/twitter2.svg',
                    title: 'Twitter',
                    description: 'Twitter 是一家美国社交网络及微博客服务的公司，致力于服务公众对话。'
                }, {
                    name: 'google',
                    logo: 'https://files.authing.co/authing-console/social-connections/google2.svg',
                    title: 'Google',
                    description: 'Google 是全球最大的搜索引擎公司。'
                }]
            })
        }, 1000);
    }
})