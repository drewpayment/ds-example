

export interface IReviewRating {
    reviewRatingId?:number,
    clientId:number,
    rating:number,
    label:string,
    description:string,

    // RELATIONSHIPS
    client?:any // this is comes from ClientDto
}