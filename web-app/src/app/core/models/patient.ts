import { Base } from './base';

export interface Patient extends Base {
  fullName: string;
  sex: string;
  identification: string;
}

export interface PatientCreateDto {
  fullName: string;
  sex: string;
  identification: string;
}
