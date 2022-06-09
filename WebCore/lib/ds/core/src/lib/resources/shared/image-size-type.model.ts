
export interface IImageSizeType {
    imageSizeTypeId:number,
    name:string,
    description:string,
    height:number,
    width:number
}

export enum ImageSizeType {
    XL = 1, // 512
    LG, // 256
    MD, // 128
    SM, // 64
    companyLogo, // 300 x 150
    companyHero // 800 x 400 
}