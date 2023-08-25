import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/dataresource': (req, res) => {
        setTimeout(() => {
            res.json(mockjs.mock({
                code: 200,
                data: {
                    totalCount: "@integer(0, 100)",
                    "items|20": [{
                        id: '@guid',
                        name: '@ctitle(5,10)',
                        code: '@name(5,10)',
                        description: '@ctitle(10,50)',
                        type: 'TREE',
                        latestModificationTime: '@datetime()'
                    }]
                }
            }))
        }, 2000)
    }
})