import { Base } from './base';
import { Speciality } from './speciality';

export interface Doctor extends Base {
  fullName: string;
  specialityId: string;
  identification: string;
  speciality?: Speciality;
}

export interface DoctorRecord extends Base {
  fullName: string;
  specialityId: string;
  identification: string;
  speciality?: string;
}

export interface DoctorCreateDto {
  fullName: string;
  specialityId: string;
  identification: string;
}
