import { request } from "@/lib/request"

const IdentityProviderTemplateService = {
    getAll: async () => {
        const { data } = await request('/api/admin/idpTemplates');
        return data;
    },

    get: async (providerName: string) => {
        const { data } = await request(`/api/admin/idpTemplates/${providerName}`);
        return data;
    }
}

export default IdentityProviderTemplateService