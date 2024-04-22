import { ChangePasswordReq, ForgotPasswordReq, ResetPasswordReq } from "@/@types/user"
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
    },

    forgotPassword: async (input: ForgotPasswordReq) => {
        const { code, data, ...response } = await request('/api/account/forgotpassword', { method: 'post', data: input })
        console.log(response)

        return response
    },

    resetPassword: async (input: ResetPasswordReq) => {
        return await request('/api/account/resetpassword', { method: 'post', data: input })
    },

    changePassword: (input: ChangePasswordReq) => request('/api/account/changepassword', { method: 'post', data: input })
}

export default AccountService