import { request } from "@/lib/request";

const IdentityProviderService = {
    getAll: async () => {
        const { data } = await request('/api/admin/idps');
        return data;
    }
}


export default IdentityProviderService