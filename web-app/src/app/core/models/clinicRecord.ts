import { Base } from "./base";
import { Doctor, DoctorRecord } from "./doctor";
import { Patient } from "./patient";

export interface ClinicRecord extends Base{
  doctorId: string;
  patientId: string;
  diagnosis: string;
  patient?: Patient;
  doctor?: DoctorRecord
}


export interface ClinicRecordCreateDto{
  doctorId: string;
  patientId: string;
  diagnosis: string;
}

