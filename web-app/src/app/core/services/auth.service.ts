import { Injectable, isDevMode, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserToken } from '../models/userToken';
import { tap } from 'rxjs';
import { User } from '../models/user';
import { TokenService } from './token.service';
import jwtDecode from 'jwt-decode';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly api = `${environment.accountServiceApi}/auth`;
  user =  signal<User|null>(null);

  constructor(private http: HttpClient, private tokenService: TokenService) {
    this.loadUser();
  }

  login(email: string, password: string) {
    const user = {
      email,
      password,
    };
    return this.http.post<UserToken>(`${this.api}/login`, user).pipe(
      tap((resp) => {
        this.tokenService.set(resp.token);
        const user: User = jwtDecode(resp.token);
        this.user.set(user);
      })
    );
  }

  logout() {
    this.tokenService.remove();
    this.user.set(null);
  }

  private loadUser(){
    const token = this.tokenService.get();
    if(token === null) return;
    const user: User = jwtDecode(token);
    this.user.set(user);
  }
}
