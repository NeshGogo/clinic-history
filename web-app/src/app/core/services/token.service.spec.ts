import { TestBed } from '@angular/core/testing';

import { TokenService } from './token.service';

describe('TokenService', () => {
  let service: TokenService;
  const name = 'token'
  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('Should set a token in localStorage', () => {
    const val = 'value';
    service.set(val);
    expect(localStorage.getItem(name)).toEqual(val);
  });

  it('Should rewrite the token in localStorage if exists', () => {
    const preVal = 'test'
    localStorage.setItem(name,preVal);
    expect(localStorage.getItem(name)).toEqual(preVal);
    const val = 'value';
    service.set(val);
    expect(localStorage.getItem(name)).toEqual(val);
  });

  it('Should get the token in localStorage', () => {
    const val = 'value';
    localStorage.setItem(name,val);
    const result = service.get();
    expect(result).toEqual(val);
  });

  it('Should remove the token in localStorage', () => {
    const val = 'value';
    localStorage.setItem(name,val);
    service.remove();
    expect(localStorage.getItem(name)).toBeNull();
  });
});
