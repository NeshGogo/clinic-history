import { Base } from "./base";
import { Doctor } from "./doctor";
import { Patient } from "./patient";

export interface ClinicRecord extends Base{
  doctorId: string;
  patientId: string;
  diagnosis: string;
  patient?: Patient;
  doctor?: Doctor
}


export interface ClinicRecordCreateDto{
  doctorId: string;
  patientId: string;
  diagnosis: string;
}

