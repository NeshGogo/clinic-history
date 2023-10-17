import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing'

import { AuthService } from './auth.service';
import { UserToken } from '../models/UserToken';
import { of } from 'rxjs';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login with email and password', (done) => {
    var userToken: UserToken = {
      token: 'sadsadas', 
      expiration: new Date()
    };
    const loginSpy = spyOn(service, 'login').and.returnValue(of(userToken))
    var result = service.login('test@asdas.com', 'asdsadas');
    expect(loginSpy).toHaveBeenCalled();
    result.subscribe(p => {
      expect(p.token).toEqual(userToken.token);
      expect(p.expiration).toEqual(userToken.expiration);
      done();
    })
  });
});
