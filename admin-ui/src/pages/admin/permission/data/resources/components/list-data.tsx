import Empty from '@/components/Empty'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { MinusCircleIcon, PackageOpenIcon, PlusIcon } from 'lucide-react'
import React, { useMemo, useState } from 'react'


type ListDataItemProps = {
    value: string
    onChange?: (e: any) => void
    onRemove?: () => void
}

const ListDataItem: React.FC<ListDataItemProps> = (({ value, onChange, onRemove }) => {
    return (
        <div className="flex items-center">
            <div className="flex-1 p-2">
                <Input type="text" value={value} placeholder="输入数组项值，最长 50 个字符" maxLength={50}
                    onChange={onChange} />
            </div>
            <Button variant="link" onClick={onRemove}>
                <MinusCircleIcon className="w-5 h-5" />
            </Button>
        </div>
    )
})

export type ListDataProps = {
    value?: string[]
    onChange: (value: any) => void
}

const ListData = React.forwardRef<{}, ListDataProps>((props, _) => {
    const {
        value: items = [],
        onChange
    } = props

    const onAddItem = () => {
        const newItems = items.concat([''])
        onChange(newItems)
    }

    const onUpdateItem = (e: any, index: number) => {
        console.log('update ', index, items)
        const value = e.target.value;
        const newItems = items.map((x, i) => {
            if (i === index) return value
            return x
        })

        onChange(newItems)
    }

    const onRemoveItem = (index: number) => {
        const newItems = items.filter((_, i) => i !== index)

        onChange(newItems)
    }

    return (
        <div>
            <p className="text-sm block mb-1 after:content-['*'] after:text-red-600">数组</p>
            <div className="w-full h-[400px] overflow-hidden bg-gray-100 rounded-md flex flex-col">
                <div className="flex-1 overflow-hidden py-4">
                    {items.length ?
                        <div className="max-h-full overflow-auto px-4">
                            {items.map((item, index) => (
                                <ListDataItem key={index} value={item}
                                    onChange={e => onUpdateItem(e, index)}
                                    onRemove={() => onRemoveItem(index)} />
                            ))}
                        </div> :
                        <div className="flex flex-col items-center justify-center text-gray-400 gap-y-4 h-full">
                            <PackageOpenIcon className="w-12 h-12" />
                            <p className="text-sm">可以在一个数组中添加任意个数据，最长 50 个字符。</p>
                        </div>
                    }
                </div>
                <div className="border-t border-gray-200 select-none text-primary cursor-pointer flex justify-center items-center py-4 text-sm transition-colors hover:bg-gray-200"
                    onClick={onAddItem}>
                    <PlusIcon className="w-4 h-4" />
                    <span>添加元素</span>
                </div>
            </div>
        </div>
    )
})

export default ListData