import mockjs from "mockjs";
import { defineMock } from "umi";

export default defineMock({
    'GET /api/admin/idps': (req, res) => {
        setTimeout(() => {
            res.status(200)
                .json(mockjs.mock({
                    code: 200,
                    "data|0-20": [{
                        id: '@guid',
                        name: '@name',
                        displayName: '@cname',
                        title: '@ctitle',
                        logo: '@image("250x250")',
                        enabled: '@boolean'
                    }]
                }))
        }, 1000);
    }
})