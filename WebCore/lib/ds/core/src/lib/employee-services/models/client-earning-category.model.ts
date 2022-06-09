import { ClientEarningCategory } from '../enums';

export interface ClientEarningCategoryDto {
    clientEarningCategoryId:number;
    earningCategoryId:ClientEarningCategory;
    description: string;
    sequence: number|null;
    isAdjustToNet: boolean;
}
