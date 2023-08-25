import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/usergroups': (req, res) => {
        const { pageSize = 20 } = req.params
        setTimeout(() => {
            res.status(200).json(mockjs.mock({
                code: 200,
                data: {
                    'totalCount': '@integer(0, 2000)',
                    'items|20': [{
                        id: '@guid',
                        name: '@name(5,10)',
                        displayName: '@ctitle(3,6)',
                        creationTime: '@datetime',
                        enabled: '@boolean'
                    }]
                }
            }))
        }, mockjs.Random.integer(300, 3000));
    }
})