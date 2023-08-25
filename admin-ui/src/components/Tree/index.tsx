import TreeNode from '@/@types/TreeNode'
import Empty from '@/components/Empty'
import { Transition } from '@headlessui/react'
import classNames from 'classnames'
import React, { useState } from 'react'
import Spin from '../Spin'
import { ChevronDown, ChevronRight, Loader } from 'lucide-react'


export interface OrgTreeProps {

    isLoading?: boolean

    treeData?: Array<TreeNode>;

    className?: string;
    /**
     * 异步加载数据
     * @returns 
     */
    onLoadData?: (node: TreeNode) => Promise<void>;

    /**
     * 点击节点触发
     * @returns 
     */
    onSelect?: (node: TreeNode) => void;

    /**
     * 展开/收缩时触发
     * @param node 
     * @param expanded 
     * @returns 
     */
    onExpand?: (node: TreeNode, expanded: boolean) => void;

    /**
     * (受控)选中的key
     */
    selectedKey?: string

    /**
     * （受控）展开的key集合
     */
    expandedKeys?: Set<string>

    renderMoreMenu?: (node: TreeNode, selected: boolean) => React.ReactNode
}


export interface OrgTreeNodeProps {
    current: TreeNode

    level: number

    /**
  * 异步加载数据
  * @returns 
  */
    onLoadData?: (node: TreeNode) => Promise<void>;

    /**
     * 点击节点触发
     * @returns 
     */
    onSelect?: (node: TreeNode) => void;

    /**
     * 展开/收缩时触发
     * @param node 
     * @param expanded 
     * @returns 
     */
    onExpand?: (node: TreeNode, expanded: boolean) => void;

    /**
     * (受控)选中的key
     */
    selectedKey?: string

    /**
     * （受控）展开的key集合
     */
    expandedKeys?: Set<string>

    renderMoreMenu?: (node: TreeNode, selected: boolean) => React.ReactNode
}



const TreeNodeItem = ({
    current,
    level,
    expandedKeys,
    selectedKey,
    renderMoreMenu,
    onExpand,
    onLoadData,
    onSelect
}: OrgTreeNodeProps) => {
    const [isLoading, setLoading] = useState(false)

    const onItemClick = (node: TreeNode) => {
        if (selectedKey === node.key) return;

        onSelect && onSelect(node)
    }

    const onItemExpand = async (node: TreeNode) => {
        setLoading(true)
        try {

            const expanded = !(expandedKeys?.has(node.key) ?? false)

            console.log('expanded', expanded)

            console.log(expandedKeys)
            if (onLoadData && expanded) {
                await onLoadData(node)
            }

            onExpand && onExpand(node, expanded)
        } finally {
            setLoading(false)
        }
    }

    const currentExpanded = expandedKeys?.has(current.key) ?? false
    const currentSeleted = !!selectedKey && selectedKey === current.key

    return (
        <>
            <div aria-selected={currentSeleted}
                className={classNames(
                    "w-full h-9 flex items-center hover:bg-gray-100 px-2 rounded cursor-pointer mb-1 transition-colors group",
                    "aria-selected:bg-blue-600 aria-selected:text-white"
                )}
                title={current.title}
            >
                <div className="flex-1 flex items-center h-full"
                    onClick={() => onItemClick(current)}>
                    {level > 1 && new Array(level - 1).fill([]).map((_, index) => (<span key={index} className="w-4"></span>))}
                    <button className="w-5 h-5 flex items-center justify-center mr-1"
                        onClick={async () => await onItemExpand(current)}>
                        {isLoading ?
                            <Loader className="w-4 h-4 animate-spin" /> : (
                                currentExpanded ?
                                    <ChevronDown className="w-4 h-4" /> :
                                    <ChevronRight className="w-4 h-4" />
                            )
                        }
                    </button>
                    <span className="flex-1 truncate ... text-sm">{current.title} </span>
                </div>
                {renderMoreMenu && renderMoreMenu(current, currentSeleted)}
            </div>

            {(current.children?.length ?? 0) > 0 && current.children!.map(node => (
                <Transition key={node.key}
                    show={currentExpanded}
                    as="div"
                    enter="transition ease-out duration-100"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0">
                    <TreeNodeItem key={node.key}
                        current={node} level={level + 1} expandedKeys={expandedKeys} selectedKey={selectedKey}
                        onExpand={onExpand} onLoadData={onLoadData} onSelect={onSelect} renderMoreMenu={renderMoreMenu} />
                </Transition>
            ))}
        </>
    )

}


const Tree: React.FC<OrgTreeProps> = (props: OrgTreeProps) => {
    const { isLoading = false, treeData, className } = props;

    const isEmpty = (treeData?.length ?? 0) <= 0

    return (
        <div className={classNames(className)}>
            <Spin spinning={isLoading}>
                <Empty isEmpty={isEmpty}>
                    <div className="w-full">
                        {treeData && treeData.map((node) =>
                            <TreeNodeItem key={node.key} {...props} current={node} level={1} />
                        )}
                    </div>
                </Empty>
            </Spin>
        </div>
    )
}

export default Tree;