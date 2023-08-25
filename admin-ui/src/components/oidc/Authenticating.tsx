import { useEffect, useRef } from "react"

export default () => {

    return (
        <div className="w-screen h-screen flex items-center">
            <div className="mx-auto">
                <h2 className="text-blue-600 text-lg mb-2">跳转中</h2>
                <p className="text-gray-600">
                    即将跳转到登录页面
                </p>
            </div>
        </div>
    )
}