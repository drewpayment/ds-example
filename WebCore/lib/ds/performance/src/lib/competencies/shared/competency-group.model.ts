

export interface ICompetencyGroup {
    competencyGroupId:number,
    clientId:number,
    name:string
}

export interface ICompetencySubGroup {
    competencySubGroupId:number,
    competencyGroupId:number,
    clientId:number,
    name:string
}