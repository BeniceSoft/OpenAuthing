import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/users': (req, res) => {
        const { pageSize = 20 } = req.params
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: {
                    'totalCount': '@integer(0, 2000)',
                    'items|20': [{
                        id: '@guid',
                        userName: '@name(5,10)',
                        nickname: '@cname(3,6)',
                        emailAddress: '@email',
                        avatar: '@image("250x250")',
                        phoneNumber: '@natural(13000000000, 15600000000)',
                        enabled: '@boolean'
                    }]
                }
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'GET /api/admin/users/:id': (req, res) => {
        const { id } = req.params
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: {
                    id,
                    userName: '@name(5,10)',
                    nickname: '@cname(3,6)',
                    emailAddress: '@email',
                    avatar: '@image("250x250")',
                    phoneNumber: '@natural(13000000000, 15600000000)'
                }
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'POST /api/admin/users': (req, res) => {
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: '@guid'
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'DELETE /api/admin/users/:id': (req, res) => {
        const { id } = req.params
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: "@boolean"
            }))
        }, mockjs.Random.integer(300, 3000));
    },
})