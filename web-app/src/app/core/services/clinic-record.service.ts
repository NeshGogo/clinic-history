import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ClinicRecord, ClinicRecordCreateDto } from '../models/clinicRecord';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ClinicRecordService {
  private readonly API = `${environment.historyServiceApi}`;

  constructor(private http: HttpClient) {}

  getAll(patientId: string) {
    return this.http.get<ClinicRecord[]>(`${this.API}/${patientId}/clinicRecords`);
  }

  add(patientId: string, record: ClinicRecordCreateDto) {
    return this.http.post<ClinicRecord>(`${this.API}/${patientId}/clinicRecords`, record);
  }
}
