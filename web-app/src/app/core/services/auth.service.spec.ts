import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AuthService } from './auth.service';
import { UserToken } from '../models/userToken';
import { TokenService } from './token.service';
import { User } from '../models/user';
import { environment } from 'src/environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let tokenService: TokenService;
  let api: string;
  let userToken: UserToken;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService, TokenService],
    });
    service = TestBed.inject(AuthService);
    tokenService = TestBed.inject(TokenService);
    httpMock = TestBed.inject(HttpTestingController);
    api = `${environment.accountServiceApi}/auth`;
    userToken = {
      token:
        'eyJ0eXAiOiJKV1QiLCJhbGciOiJFUzI1NiIsImtpZCI6ImJhMGVjYTI2MDkwZmNjOWZlOGNhZTI1ZDRkNmUyMmVkIn0.eyJuYW1lIjoiVGVzdCBUZXN0IiwiZXhwIjoxNjk3NjQwNDY4LCJlbWFpbCI6IlRlc3RAdGVzdC5jb20iLCJpZCI6IjEyMTIzIn0.f-PxDUao4CmmdvX1ezeO5xlo9rdld2nY-6Q1wj8-McvjcMvEP66NLsjiDiaM-5UYbqFoWdcaw-XBNcyjcmvCog',
      expiration: new Date(),
    };
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add token to local storage and property user$ is different to null', (done) => {
    let user: User = {
      email: 'Test@test.com',
      name: 'Test Test',
      exp: 1697640468,
      id: '12123',
    };

    const tokenServiceSpy = spyOn(tokenService, 'set').and.returnValue();
    service.login('test@asdas.com', 'asdsadas').subscribe((p) => {
      expect(p).toEqual(userToken);
      expect(tokenServiceSpy).toHaveBeenCalledWith(userToken.token);
      expect(service.user()).toEqual(user);
      done();
    });

    const req = httpMock.expectOne(`${api}/login`);
    expect(req.request.method).toBe('POST');
    req.flush(userToken);
  });

  it('Should set user to null and remove token when logout', (done) => {
    const tokenServiceSpy = spyOn(tokenService, 'remove').and.callThrough();
    service.login('test@asdas.com', 'asdsadas').subscribe(() => {
      expect(service.user()).toBeTruthy();
      service.logout();
      expect(service.user()).toBeNull();
      expect(tokenServiceSpy).toHaveBeenCalled();
      done();
    });

    const req = httpMock.expectOne(`${api}/login`);
    expect(req.request.method).toBe('POST');
    req.flush(userToken);
  });
});
