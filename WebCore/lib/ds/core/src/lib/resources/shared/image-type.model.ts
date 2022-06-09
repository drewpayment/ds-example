
export interface IImageType {
    imageTypeId:number,
    name:string,
    description:string
}

export enum ImageType {
    profile = 1,
    companyLogo = 2,
    companyHero = 3
}