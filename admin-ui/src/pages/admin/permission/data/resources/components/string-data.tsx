import React, { useMemo, useState } from 'react'


export type StringDataProps = {
    value?: string[]
    onChange: (value: any) => void
}

const StringData = React.forwardRef<{}, StringDataProps>((props, _) => {

    return (
        <div>
            <p className="text-sm block mb-1 after:content-['*'] after:text-red-600">字符串值</p>
            <textarea rows={4} maxLength={200}
                className="w-full border-none rounded max-h-40 min-h-[50px] bg-gray-100 dark:bg-gray-700 text-sm transition duration-300 focus:ring-2 aria-invalid:ring-red-500"
                placeholder="请输入字符串值"
                {...props} />
        </div>
    )
})

export default StringData