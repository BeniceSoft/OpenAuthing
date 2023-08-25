import { CurrentUserInfo } from "./auth"

export default interface InitialStateModel {
    theme?: string
    isAuthenticated?: boolean
    currentUser?: CurrentUserInfo
}