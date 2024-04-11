import { defineMock } from "umi";
import mockjs from 'mockjs';

export default defineMock({
    'POST /api/account/login': (req, res) => {
        const requiresTwoFactor = mockjs.Random.boolean()
        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: {
                    loginSuccess: !requiresTwoFactor,
                    requiresTwoFactor: requiresTwoFactor,
                    returnUrl: '/',
                    userInfo: { nickname: mockjs.Random.cname(), userName: mockjs.Random.name() }
                }
            })
        }, mockjs.Random.integer(300, 3000));
    },

    'POST /api/account/loginwith2fa': (req, res) => {

        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: {
                    returnUrl: '/',
                    userInfo: { nickname: mockjs.Random.cname(), userName: mockjs.Random.name() }
                }
            })
        }, mockjs.Random.integer(300, 3000));
    },

    'GET /api/account/towFactorAuthentication': (req, res) => {
        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: {
                    is2FaEnabled: mockjs.Random.boolean(),
                    hasAuthenticator: true,
                    isMachineRemembered: true,
                    recoveryCodesLeft: 8
                }
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'GET /api/account/generateAuthenticatorUri': (req, res) => {
        setTimeout(() => {
            const temp = encodeURIComponent('BeniceSoft OpenAuthing')
            const uri = `otpauth://totp/${temp}:${encodeURIComponent('13000000001')}?secret=${encodeURIComponent('1234 1234 1234 1234 1234 1234 1234 1234')}&issuer=${temp}&digits=6`
            res.status(200).json({
                code: 200,
                data: {
                    authenticatorUri: uri
                }
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'POST /api/account/enableAuthenticator': (req, res) => {
        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: {
                    recoveryCodes: ['UUHUU-UIIOU', 'UHUHJ-UIOJJ', 'POPIO-89JUI']
                }
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'GET /api/account/getexternalloginproviders': (_, res) => {
        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: [{
                    providerName: 'Feishu',
                    name: 'Feishu',
                    displayName: '飞书'
                }]
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'POST /api/account/uploadAvatar': (req, res) => {
        console.log(req)

        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: true
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'GET /api/account/me': (req, res) => {

        setTimeout(() => {
            res.status(200).json({
                code: 200,
                data: {
                    avatar: mockjs.Random.image('200x200'),
                    nickname: mockjs.Random.cname(),
                    userName: mockjs.Random.name()
                }
            })
        }, mockjs.Random.integer(100, 3000));
    },

    'GET /api/account/profile': (req, res) => {
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                'data': {
                    id: '@guid',
                    avatar: "@image('200x200')",
                    nickname: '@cname',
                    userName: '@name',
                    gender: '@gender',
                    jobTitle: '@ctitle',
                    phoneNumber: '',
                    emailAddress: '@email',
                    creationTime: '@date'
                }
            }))
        }, mockjs.Random.integer(100, 3000));
    },

    '/api/account/getRecoveryCodes': (req, res) => {

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                'data|20': [/[A-Z]{5}-[A-Z]{5}/]
            }))
        }, mockjs.Random.integer(100, 3000));
    }
})