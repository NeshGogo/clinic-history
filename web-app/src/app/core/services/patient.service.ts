import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Patient, PatientCreateDto } from '../models/patient';
import { Pagination } from '../models/pagination';
import { query } from '@angular/animations';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private readonly API = `${environment.historyServiceApi}/patients`;

  constructor(private http: HttpClient) {}

  getAll(pagination: Pagination) {
    return this.http.get<Patient[]>(`${this.API}?page=${pagination.page}&size=${pagination.size}`);
  }

  exists(identification: string) {
    return this.http.get<boolean>(`${this.API}/exists/${identification}`);
  }

  add(patient: PatientCreateDto) {
    return this.http.post<Patient>(this.API, patient);
  }
}
