export class ResponseModel<T> {
    message: string = '';
    latestId: string | '' = '';
    totalCount: number = 0;
    data: T | null = null;
}