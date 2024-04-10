import { Outlet } from "umi"
import { Toaster } from "react-hot-toast";

export default () => {

    return (
        <div>
            <Outlet />
            <Toaster />
        </div>
    )
}