
export default interface TreeNode {
    key: string
    title: string
    parentId: string | null
    children?: Array<TreeNode>
}