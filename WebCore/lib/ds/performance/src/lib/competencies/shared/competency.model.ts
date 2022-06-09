import { ICompetencyGroup } from "./competency-group.model";
import { ICompetencyModel } from "./competency-model.model";


export interface ICompetency {
    competencyId:number,
    clientId:number,
    name:string,
    description:string,
    competencyGroupId?:number|null,
    isCore:boolean,
    isArchived:boolean,
    difficultyLevel?:number|null,
    competencyGroup?:ICompetencyGroup,
    models?:ICompetencyModel[],
    canRemove?:boolean /** returned on get calls, calculation to determine if the competency can be deleted */
}