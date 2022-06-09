export interface ClientAlert {
    alertId: number,
    datePosted: Date,
    alertText: string,
    alertLink: string,
    dateStartDisplay: Date,
    dateEndDisplay: Date,
    alertType: number,
    modified: Date,
    modifiedBy: string,
    alertCategoryId: number,
    title: string,


    //custom
    datePostedCustom: string,
    datePostedCustom2: string,
    
}