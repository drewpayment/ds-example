export interface ILengthOfTime {
  lengthOfTime: string;
  id: number;
  disabled: boolean;
  selected?: boolean;
}

export interface IMovies {
  id: number;
  firstName?: string;
  lastName?: string;
  character?: string,  
  movie: string;
  year: number;
  selected?: boolean;
  isEditView?: boolean;
  quote?: string;
  avatar?: string;
}

export interface IYears {
  id: number;
  year: number;
}