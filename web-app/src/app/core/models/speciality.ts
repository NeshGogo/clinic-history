import { Base } from "./base";

export interface Speciality extends Base {
  name: string;
  description?: string;
}

export interface SpecialityCreateDto {
  name: string;
  description?: string;
}