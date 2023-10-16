import { Injectable, isDevMode } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserToken } from '../models/UserToken';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly url = isDevMode()? `http://localhost:5173/api` : '';

  constructor(private http: HttpClient) { }

  login(email: string, password:string){
    const user = {
      email,
      password,
    }
    return this.http.post<UserToken>(`${this.url}/auth`, user);
  }
}
