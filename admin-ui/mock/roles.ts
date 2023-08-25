import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/roles': (req, res) => {
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: {
                    'totalCount': '@integer(0, 2000)',
                    'items|20': [{
                        id: '@guid',
                        name: '@name(5,10)',
                        displayName: '@cname(3,6)',
                        enabled: '@boolean',
                        isBuiltInSystem: '@boolean',
                        description: '@ctitle(10, 100)'
                    }]
                }
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'GET /api/admin/roles/:id': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: {
                    id: id,
                    name: '@name(5,10)',
                    displayName: '@cname(3,6)',
                    enabled: '@boolean',
                    isBuiltSystem: '@boolean',
                    description: '@ctitle(10, 100)'
                }
            }))
        }, mockjs.Random.integer(300, 1000));
    },

    'POST /api/admin/roles': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: '@guid'
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'PUT /api/admin/roles/:id': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: true
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'DELETE /api/admin/roles/:id': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: true
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'PUT /api/admin/roles/:id/toggle-enabled': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: true
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'GET /api/admin/roles/:id/subjects': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                'data|0-50': [{
                    subjectType: 0,
                    subjectId: '@guid',
                    name: '@cname',
                    creationTime: '@datetime'
                }]
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'PUT /api/admin/roles/:id/subjects': (req, res) => {
        const { id } = req.params

        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: true
            }))
        }, mockjs.Random.integer(300, 3000));
    }
})