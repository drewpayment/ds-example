export interface FilterData{
    filters?: FilterItem[];
    startDate?: Date;
    endDate?: Date;
}
export interface FilterItem{
    name: string;
    filter: string;
}