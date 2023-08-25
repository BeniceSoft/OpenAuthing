import Spin from "@/components/Spin"
import { ArrowLeft } from "lucide-react"
import { useEffect } from "react"
import { Navigate, useModel, useParams, history } from "umi"

export default () => {
    const { id } = useParams()
    if (id == undefined) return (
        <Navigate replace={true} to='/admin/org/user-groups' />
    )

    const { userGroup, loading, fetch } = useModel('admin.user-groups.detail.index')

    useEffect(() => {
        fetch(id)
    }, [id])

    return (
        <div className="w-full">
            <Spin spinning={loading}>
                <div className="mb-2">
                    <span onClick={history.back}
                        className="cursor-pointer inline-flex items-center text-sm gap-x-1 text-gray-400 hover:text-blue-600 transition-colors duration-300">
                        <ArrowLeft className="w-4 h-4" />
                        返回
                    </span>
                </div>

                {userGroup &&
                    <div>
                        {userGroup.name}
                    </div>
                }
            </Spin>
        </div>
    )
}