import { Injectable, isDevMode } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserToken } from '../models/UserToken';
import { BehaviorSubject, tap } from 'rxjs';
import { User } from '../models/user';
import { TokenService } from './token.service';
import jwtDecode from 'jwt-decode';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly api = `${environment.accountServiceApi}/auth`;
  private user = new BehaviorSubject<User | null>(null);
  user$ = this.user.asObservable();

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  login(email: string, password: string) {
    const user = {
      email,
      password,
    };
    return this.http.post<UserToken>(`${this.api}/login`, user).pipe(
      tap((resp) => {
        this.tokenService.set(resp.token);
        var user: User = jwtDecode(resp.token);
        this.user.next(user);
      })
    );
  }

  logout() {
    this.tokenService.remove();
    this.user.next(null);
  }
}
