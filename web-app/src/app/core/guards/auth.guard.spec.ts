import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router, UrlTree } from '@angular/router';

import { authGuard } from './auth.guard';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthComponent } from 'src/app/auth/auth.component';
import { AuthService } from '../services/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';

describe('authGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => TestBed.runInInjectionContext(() => authGuard(...guardParameters));
  let router: Router;
  let authService: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([{ path: '', component: AuthComponent }])],
      providers: [authGuard, AuthService],
    });
    router = TestBed.inject(Router);
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });

  it('should be false and redirect to login page', (done) => {
    let route = jasmine.createSpyObj('Route', ['']);
    let state = jasmine.createSpyObj('RouterStateSnapshot', ['']);
    const routeSpy = spyOn(router, 'navigate').and.callThrough();
    const guard = executeGuard(route, state) as boolean;
    expect(routeSpy).toHaveBeenCalledWith(['/']);
    expect(guard).toBeFalse();
    done();
  });

  it('should be true and redirect to login page', (done) => {
    authService = TestBed.inject(AuthService);
    const user: User = {
      id: '1231asda123',
      name: 'test',
      email: 'test@test.com',
      exp: 12312,
    };
    authService.user.set(user);
    const route = jasmine.createSpyObj('Route', ['']);
    const state = jasmine.createSpyObj('RouterStateSnapshot', ['']);
    const routeSpy = spyOn(router, 'navigate').and.callThrough();
    const guard = executeGuard(route, state) as boolean;
    expect(routeSpy).not.toHaveBeenCalled();
    expect(guard).toBeTruthy();
    done();
  });
});
