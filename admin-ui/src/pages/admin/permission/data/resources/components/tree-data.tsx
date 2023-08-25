import { Button } from '@/components/ui/button'
import { Tooltip, TooltipArrow, TooltipContent, TooltipProvider, TooltipTrigger } from '@/components/ui/tooltip'
import { CheckIcon, FilesIcon, PackageOpenIcon, PenLine, PlayIcon, PlusIcon, Trash2Icon } from 'lucide-react'
import React, { useEffect, useRef, useState } from 'react'
import TreeNodeModal, { TreeNodeModalRef } from './tree-node-modal'
import { Transition } from '@headlessui/react'
import classNames from 'classnames'
import { toast } from 'react-hot-toast'

export type TreeNode = {
    name: string
    key: string
    value: string
    path: string
    children?: TreeNode[]
    parent?: TreeNode
}

export type TreeDataProps = {
    value?: TreeNode[]
    onChange: (value: any) => void
}

type TreeDataItemProps = {
    node: TreeNode
    parent?: TreeNode
    level: number
    index: number,
    hasNext: boolean
    onAddOrEdit: (current?: TreeNode, parent?: TreeNode) => void;
    onRemove: (current: TreeNode, parent?: TreeNode) => void;
} & React.PropsWithChildren

const TreeDataItem: React.FC<TreeDataItemProps> = ({ node, parent, index, level, children, hasNext, onAddOrEdit, onRemove }) => {
    const [collapsed, setCollapsed] = useState<boolean>(false);
    const [copied, setCopied] = useState<boolean>(false);

    useEffect(() => {
        if (!copied) return
        const timer = setTimeout(() => {
            setCopied(false)
        }, 3000)

        return () => clearInterval(timer)
    }, [copied])

    const renderIcon = (level: number, hasNext: boolean, hasChildren: boolean): React.ReactNode => {
        if (hasChildren) {
            return (
                <span className="w-5 h-5 ml-[1px] flex items-center justify-center mr-1" onClick={() => setCollapsed(!collapsed)}>
                    <PlayIcon className={classNames("w-3 h-3 transition-transform fill-current", !collapsed && "rotate-90")} />
                </span>
            )
        } else {
            if (level === 0 && index === 0 && !hasNext) {
                return (
                    <span className="inline-block w-5 h-5 ml-[1px] mr-1"></span>
                )
            }

            return (
                <>
                    {hasNext && <span className='w-[1px] h-full bg-gray-400 absolute left-3'></span>}
                    <span className="h-5 w-4 ml-3 border-l border-b border-gray-400 rounded-bl self-start mr-2"></span>
                </>
            )
        }
    }

    const onCopy = (path: string) => {
        navigator.clipboard.writeText(path)

        setCopied(true)
    }

    return (
        <div key={node.path} className="w-full">
            <div className="flex h-10 items-center cursor-pointer px-2 rounded group transition-colors hover:bg-gray-200">
                <span className="inline-block" style={{ width: level * 20 + 'px' }}></span>
                <div className="flex-1 h-full w-full flex items-center text-sm relative">
                    {renderIcon(level, hasNext, !!node.children?.length)}
                    <div className="flex items-center text-sm gap-x-2">
                        <span>{node.name}</span>
                        <span className="text-gray-400">{node.path}</span>
                        <TooltipProvider>
                            <Tooltip>
                                <TooltipTrigger onClick={() => onCopy(node.path)} className="text-primary">
                                    {copied ?
                                        <CheckIcon className="w-3 h-3" /> :
                                        <FilesIcon className="w-3 h-3" />
                                    }
                                </TooltipTrigger>
                                <TooltipContent>
                                    <span className="text-xs">点击复制路径</span>
                                    <TooltipArrow />
                                </TooltipContent>
                            </Tooltip>
                        </TooltipProvider>
                    </div>
                </div>
                <div className=" hidden group-hover:flex">
                    <TooltipProvider>
                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Button variant="ghost" type="button" onClick={() => onAddOrEdit(undefined, node)}>
                                    <PlusIcon className="w-4 h-4" />
                                </Button>
                            </TooltipTrigger>
                            <TooltipContent className="bg-black/80 text-white">
                                <span className="text-xs">添加子节点</span>
                                <TooltipArrow />
                            </TooltipContent>
                        </Tooltip>

                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Button variant="ghost" type="button" onClick={() => onAddOrEdit(node, node.parent)}>
                                    <PenLine className="w-4 h-4" />
                                </Button>
                            </TooltipTrigger>
                            <TooltipContent className="bg-black/80 text-white">
                                <span className="text-xs">编辑节点</span>
                                <TooltipArrow />
                            </TooltipContent>
                        </Tooltip>

                        <Tooltip>
                            <TooltipTrigger asChild>
                                <Button variant="ghost" type="button" onClick={() => onRemove(node, parent)}>
                                    <Trash2Icon className="w-4 h-4" />
                                </Button>
                            </TooltipTrigger>
                            <TooltipContent className="bg-black/80 text-white">
                                <span className="text-xs">删除节点</span>
                                <TooltipArrow />
                            </TooltipContent>
                        </Tooltip>
                    </TooltipProvider>
                </div>
            </div>
            <Transition
                as="div"
                className={classNames("grid transition-all", !collapsed ? "grid-rows-[1fr]" : "grid-rows-[0fr]")}
                show={!collapsed}
                unmount={false}
                enterFrom="grid-rows-[0fr]"
                enterTo="grid-rows-[1fr]"
                leaveFrom="grid-rows-[1fr]"
                leaveTo="grid-rows-[0fr]">
                <div className="w-full overflow-hidden relative">
                    {hasNext && <span className="z-20 block absolute w-[1px] h-full bg-gray-400" style={{ left: `${(level + 1) * 20}px` }}></span>}
                    {children}
                </div>
            </Transition>
        </div >
    )
}

const TreeData = React.forwardRef<{}, TreeDataProps>(({
    value: nodes = [],
    onChange
}, _) => {
    const modalRef = useRef<TreeNodeModalRef>(null)

    const openTreeNodeModal = (currentNode?: TreeNode, parentNode?: TreeNode) => {
        if (modalRef.current) {
            modalRef.current.open(currentNode, parentNode)
        }
    }

    const createOrUpdateNode = (data: any) => {
        console.log('createOrUpdateNode', data)
        const { parentNode, currentNode, name, key, value } = data;

        // 校验同一级不能有相同的key
        if (parentNode) {
            if (parentNode.children?.findIndex((x: any) => currentNode?.key !== x.key && x.key === key) >= 0) {
                toast.error(`同一级不能存在相同的键`)
                return
            }
        } else {
            if (nodes.findIndex((x: any) => currentNode?.key !== x.key && x.key === key) >= 0) {
                toast.error(`同一级不能存在相同的键`)
                return
            }
        }

        const path = (parentNode?.path ?? '') + '/' + key

        // edit
        if (currentNode) {
            currentNode.name = name
            currentNode.key = key
            currentNode.value = value
            currentNode.path = path
        } else {
            const newNode = { name, key, value, path, children: [] }
            console.log("newNode", newNode)
            if (parentNode) {
                if (!parentNode.children) parentNode.children = []
                parentNode.children.push(newNode)
            } else {
                nodes.push(newNode)
            }
        }

        onChange(nodes)
    }

    const removeNode = (currentNode: TreeNode, parentNode?: TreeNode) => {
        if (parentNode) {
            const index = parentNode.children?.findIndex(node => node.key === currentNode.key) ?? -1;
            if (index !== -1) {
                // 从父节点的子节点列表中删除指定节点
                parentNode.children?.splice(index, 1);
            }
        } else {
            const index = nodes.findIndex(node => node.key === currentNode.key);
            if (index !== -1) {
                nodes.splice(index, 1);
            }
        }

        onChange(nodes)
    }

    const renderNodes = (nodes?: TreeNode[], parentNode?: TreeNode, level: number = 0) => {
        const length = nodes?.length ?? 0
        return nodes?.map((x, i) => (
            <TreeDataItem key={x.path} onAddOrEdit={openTreeNodeModal} onRemove={removeNode}
                node={x} index={i} level={level} hasNext={i < length - 1} parent={parentNode}>
                {renderNodes(x.children, x, level + 1)}
            </TreeDataItem>
        ))
    }

    return (
        <div>
            <p className="text-sm block mb-1 after:content-['*'] after:text-red-600">树</p>
            <div className="w-full h-[400px] overflow-hidden bg-gray-100 rounded-md flex flex-col">
                <div className="flex-1 overflow-hidden py-4">
                    {nodes.length ?
                        <div className="max-h-full overflow-auto px-4">
                            {renderNodes(nodes)}
                        </div> :
                        <div className="flex flex-col items-center justify-center text-gray-400 gap-y-4 h-full">
                            <PackageOpenIcon className="w-12 h-12" />
                            <p className="text-sm">暂无数据</p>
                        </div>
                    }
                </div>
                <div className="border-t border-gray-200 select-none text-primary cursor-pointer flex justify-center items-center py-4 text-sm transition-colors hover:bg-gray-200"
                    onClick={() => openTreeNodeModal()}>
                    <PlusIcon className="w-4 h-4" />
                    <span>添加根节点</span>
                </div>
            </div>
            <TreeNodeModal ref={modalRef} onSave={createOrUpdateNode} />
        </div>
    )
})

export default TreeData