import { Button } from "@/components/ui/button"
import PageHeader from "../../components/PageHeader"
import { Tabs, TabsContent, TabsNav, TabsTrigger } from "@/components/ui/tabs"

export default () => {
    return (
        <div className="w-full h-full flex flex-col">
            <PageHeader title="常规资源权限"
                description="常规资源权限用于管理你的业务系统中以 API 为代表的类型资源，你可以创建资源、定义操作，并将资源与操作授权给角色。"
                rightRender={() => (
                    <Button>
                        创建常规资源
                    </Button>
                )} />

            <main>
                <Tabs defaultValue="index">
                    <TabsNav className="border-b">
                        <TabsTrigger value="index">常规资源</TabsTrigger>
                        <TabsTrigger value="auth">授权管理</TabsTrigger>
                    </TabsNav>
                    <div>
                        <TabsContent value="index">
                            index
                        </TabsContent>
                        <TabsContent value="auth">
                            auth
                        </TabsContent>
                    </div>
                </Tabs>
            </main>
        </div>
    )
}