import { Button } from "@/components/ui/button"
import PageHeader from "../../components/PageHeader"
import { Dialog, DialogClose, DialogContent, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { history } from "umi"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Suspense, lazy, useState } from "react"
import { Tabs, TabsContent, TabsNav, TabsTrigger } from "@/components/ui/tabs"
import Spin from "@/components/Spin"

const ResourceManagement = lazy(() => import("./resources/management"))
const DataStrategy = lazy(() => import("./resources/strategy"))

export const DataTypes: Record<string, { title: string, description: string }> = {
    "TREE": {
        title: "树结构",
        description: '适用场景：菜单'
    },
    "ARRAY": {
        title: "数组结构",
        description: '适用场景：数据集合'
    },
    "STRING": {
        title: "字符串",
        description: '适用场景：路径指代'
    },
}

export default () => {
    const [dataType, setDataType] = useState<string>()

    const onDataTypeChange = (value: string) => {
        setDataType(value)
    }

    const onCreate = () => {
        if (!dataType) {
            return
        }
        history.push({
            pathname: '/admin/permission/data/resources/create',
            search: '?type=' + dataType
        })
    }

    return (
        <div className="w-full h-full flex flex-col">
            <PageHeader title="数据资源权限"
                description="数据资源权限可以用来管理你的业务系统中的场景化的资源与授权，例如具有层级结构的应用菜单、文档目录访问操作权限等等，并且提供策略化授权与完整的权限视图。"
                rightRender={() => (
                    <div className="flex gap-x-2">
                        <Button type="button" variant="secondary" onClick={() => history.push('/admin/permission/data/auth/create')}>授权</Button>
                        <Dialog>
                            <DialogTrigger asChild={true}>
                                <Button>
                                    创建数据资源
                                </Button>
                            </DialogTrigger>
                            <DialogContent className="sm:max-w-[700px] sm:min-w-[700px]">
                                <DialogHeader>
                                    <DialogTitle>创建数据资源</DialogTitle>
                                </DialogHeader>
                                <div className="py-4">
                                    <p className="text-sm text-gray-600 mb-2">选择数据资源类型</p>
                                    <RadioGroup value={dataType} className="grid grid-cols-3 gap-x-4 h-32"
                                        onValueChange={onDataTypeChange}>
                                        {Object.keys(DataTypes).map(x => (
                                            <label key={x} htmlFor={x} className="bg-gray-100/60 border-transparent flex flex-col border-2 justify-end p-4 rounded cursor-pointer transition hover:shadow-lg [&:has([data-state=checked])]:border-primary">
                                                <RadioGroupItem id={x} value={x} className="sr-only" />
                                                <h2 className="text-sm font-medium">{DataTypes[x].title}</h2>
                                                <p className="text-sm text-gray-600">{DataTypes[x].description}</p>
                                            </label>
                                        ))}
                                    </RadioGroup>
                                </div>
                                <DialogFooter>
                                    <DialogClose asChild>
                                        <Button variant="secondary">取消</Button>
                                    </DialogClose>
                                    <Button variant="default" onClick={onCreate} disabled={!dataType}>创建</Button>
                                </DialogFooter>
                            </DialogContent>
                        </Dialog>
                    </div>
                )} />

            <Tabs className="flex-1 flex flex-col overflow-hidden gap-y-4" defaultValue="management">
                <TabsNav className="border-b">
                    <TabsTrigger value="management">数据资源</TabsTrigger>
                    <TabsTrigger value="strategy">数据策略</TabsTrigger>
                    <TabsTrigger value="view">权限视图</TabsTrigger>
                </TabsNav>
                <Suspense fallback={<Spin spinning={true} />}>
                    <TabsContent asChild value="management">
                        <ResourceManagement />
                    </TabsContent>
                    <TabsContent asChild value="strategy">
                        <DataStrategy />
                    </TabsContent>
                    <TabsContent asChild value="view">
                        view
                    </TabsContent>
                </Suspense>
            </Tabs>
        </div>
    )
}