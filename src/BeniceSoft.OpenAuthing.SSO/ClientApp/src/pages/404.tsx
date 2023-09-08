import Lottie from "lottie-react";
import AnimationData from '@/assets/animations/not-found.json'
import { Link } from "umi";

export default function () {
    return (
        <div className="h-screen w-screen pt-32">
            <div className="mx-auto w-[500px]">
                <Lottie animationData={AnimationData} />
            </div>
            <div className="text-center mt-8">
                <Link to="/" className="text-white text-sm bg-blue-500 p-2 px-4 rounded">
                    回到首页
                </Link>
            </div>
        </div>
    )
}