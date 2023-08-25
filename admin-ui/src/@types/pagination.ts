export class PaginationValues {
    pageIndex: number = 1
    pageSize: number = 20

    constructor(pageIndex: number = 1, pageSize: number = 20) {
        this.pageIndex = pageIndex
        this.pageSize = pageSize
    }
}