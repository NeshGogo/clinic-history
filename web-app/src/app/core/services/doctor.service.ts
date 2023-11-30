import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Doctor, DoctorCreateDto } from '../models/doctor';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DoctorService {
  private readonly api = `${environment.doctorServiceApi}/doctors`;
  private readonly apiHistory = `${environment.historyServiceApi}/doctors`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Doctor[]>(this.api);
  }

  getPublic() {
    return this.http.get<Doctor[]>(`${this.api}/active`);
  }

  getInHistory(){
    return this.http.get<Doctor[]>(`${this.apiHistory}`);
  }

  add(doctor: DoctorCreateDto) {
    return this.http.post<Doctor>(this.api, doctor);
  }

  update(id: string, doctor: DoctorCreateDto) {
    return this.http.put<Doctor>(`${this.api}/${id}`, doctor);
  }

  ActiveOrDisactive(id: string) {
    return this.http.put<void>(`${this.api}/ActiveOrDisactive/${id}`, {});
  }
}
