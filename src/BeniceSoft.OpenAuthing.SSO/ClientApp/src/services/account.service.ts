import { request } from "@/lib/request"

const AccountService = {
    uploadAvatar: async (avatarBlob: Blob) => {
        const formData = new FormData()
        formData.append('file', avatarBlob)
        await request('/api/account/uploadavatar', {
            method: 'put',
            data: formData,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
    },

    getProfile: async () => {
        return await request('/api/account/profile')
    }
}

export default AccountService