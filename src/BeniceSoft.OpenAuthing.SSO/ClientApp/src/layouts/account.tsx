import { Outlet } from "umi";

export default function () {
    return (
        <div className="h-screen w-screen p-0 flex items-center justify-center bg-gradient-to-b from-[#e4eaff] to-white via-40%">
            <Outlet />
        </div>
    )
}