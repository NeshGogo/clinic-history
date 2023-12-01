import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ClinicRecord, ClinicRecordCreateDto } from '../models/clinicRecord';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ClinicRecordService {
  private readonly API = `${environment.historyServiceApi}/ClinicRecords`;

  constructor(private http: HttpClient) {}

  add(record: ClinicRecordCreateDto) {
    return this.http.post<ClinicRecord>(this.API, record);
  }
}
