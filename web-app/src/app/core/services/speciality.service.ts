import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Speciality, SpecialityCreateDto } from '../models/speciality';

@Injectable({
  providedIn: 'root'
})
export class SpecialityService {
  private readonly api = `${environment.doctorServiceApi}/specialities`;
  
  constructor(private http: HttpClient) { }

  getAll(){
    return this.http.get<Speciality[]>(this.api);
  }

  add(speciality: SpecialityCreateDto) {
    return this.http.post<Speciality>(this.api, speciality);
  }

  update(id: string, speciality: SpecialityCreateDto) {
    return this.http.put<Speciality>(`${this.api}/${id}`, speciality);
  }
}
