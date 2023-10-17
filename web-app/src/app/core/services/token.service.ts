import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly tokenName = 'token';

  constructor() { }

  set(token: string){
    localStorage.setItem(this.tokenName, token);
  }

  get() {
    return localStorage.getItem(this.tokenName);
  }

  remove() {
    localStorage.removeItem(this.tokenName);
  }

}
