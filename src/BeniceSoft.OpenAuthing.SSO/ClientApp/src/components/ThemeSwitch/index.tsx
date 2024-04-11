import { MoonStarIcon, SunIcon } from "lucide-react"
import { useEffect } from "react"
import { HSThemeSwitch } from 'preline/preline'

type ThemeSwitchProps = {
    hiddenText?: boolean
}


const ThemeSwitch = ({
    hiddenText = false
}: ThemeSwitchProps) => {

    useEffect(() => {
        HSThemeSwitch.autoInit()
    }, [])

    return (
        <div>
            <button type="button"
                className="hs-dark-mode hs-dark-mode-active:hidden inline-flex items-center gap-x-2 p-2 bg-gray-100 rounded-full text-s hover:bg-gray-200"
                data-hs-theme-click-value="dark">
                <MoonStarIcon className="w-4 h-4" />
                {!hiddenText && "Dark"}
            </button>
            <button type="button"
                className="hs-dark-mode hs-dark-mode-active:inline-flex hidden items-center gap-x-2 p-2 bg-gray-600 rounded-full text-sm text-white hover:bg-gray-500"
                data-hs-theme-click-value="light">
                <SunIcon className="w-4 h-4" />
                {!hiddenText && "Light"}
            </button>
        </div>
    )
}

export default ThemeSwitch