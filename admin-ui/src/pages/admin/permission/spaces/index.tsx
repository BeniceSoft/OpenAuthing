import { Button } from "@/components/ui/button"
import PageHeader from "../../components/PageHeader"
import { Table } from "@/components/Table"
import { history } from "umi"

export default () => {
    return (
        <div className="w-full h-full flex flex-col overflow-hidden">
            <PageHeader title="权限空间" description="可以在权限空间中创建角色、资源、管理权限，不同权限空间中的角色和资源相互独立。" rightRender={() => (
                <Button onClick={() => history.push('/admin/permission/spaces/create')}>
                    创建权限空间
                </Button>
            )} />

            <div className="flex-1 overflow-hidden">
                <Table<any> columns={[{
                    title: '显示名',
                    dataIndex: 'displayName',
                    key: 'displayName'
                }, {
                    title: '名称',
                    dataIndex: 'name',
                    key: 'name'
                }, {
                    title: '描述',
                    dataIndex: 'description',
                    key: 'description',
                    width: 'w-80'
                }, {
                    title: '操作',
                    dataIndex: 'id',
                    key: 'id',
                    width: 'w-20'
                }]} />
            </div>
        </div>
    )
}