import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/applications': (req, res) => {
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                'data|10-40': [{
                    id: '@guid',
                    clientId: '@string(10,20)',
                    displayName: '@ctitle(2,5)'
                }]
            }))
        }, mockjs.Random.integer(300, 3000));
    },

    'POST /api/admin/applications': (req, res) => {
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: '@guid'
            }))
        }, mockjs.Random.integer(300, 3000));
    }
})