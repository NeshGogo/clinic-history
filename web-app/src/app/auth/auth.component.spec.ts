import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from '../core/services/auth.service';

import { AuthComponent } from './auth.component';
import { of, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { By } from '@angular/platform-browser';
import { Router } from '@angular/router';

describe('AuthComponent', () => {
  let component: AuthComponent;
  let fixture: ComponentFixture<AuthComponent>;
  let service: AuthService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AuthComponent, HttpClientTestingModule, RouterTestingModule.withRoutes([
        { path: 'home', component: AuthComponent },
      ])],
      providers: [AuthService]
    });
    fixture = TestBed.createComponent(AuthComponent);
    component = fixture.componentInstance;``
    fixture.detectChanges();
  });

  describe('When component is create', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('Should build the form', () => {
      const keys = ['email', 'password'];
      const formkeys = Object.keys(component.form?.controls || {});
      expect(formkeys).toEqual(keys);
    });
  })
  
  describe('When submit method is executed', () => {
    beforeEach(() =>{
      service = TestBed.inject(AuthService);
      router = TestBed.inject(Router);
    });

    it('should not try to login because btn is disabled', () => {
      const signInSpy = spyOn(service, 'login');
      component.submit(new Event('click'));
      expect(signInSpy).not.toHaveBeenCalled();
    });

    it('Should show an alert when login is invalid', () => {
      const loginSpy = spyOn(service, 'login').and
        .returnValues(throwError(() => new HttpErrorResponse({status: 400})));
      const showAlertSpy = spyOn(component, 'showAlert');
      component.form?.get('email')?.setValue('test@test.com');
      component.form?.get('password')?.setValue('te21312');
      component.submit(new Event('click'));
      expect(loginSpy).toHaveBeenCalled();
      expect(showAlertSpy).toHaveBeenCalled();
    });

    it('Should disable submit btn and show error if form is invalid', () => {
      component.form?.get('email')?.setValue('test.com');
      const btn : HTMLButtonElement = fixture.debugElement.query(By.css('button')).nativeElement;
      expect(component.form?.get('email')?.hasError('email')).toBeTruthy();
      expect(component.form?.get('password')?.hasError('required')).toBeTruthy();
      expect(component.form?.invalid).toBeTruthy();
      expect(btn.disabled).toBeTruthy();
    });

    it('Should navigate to home if authentication is success', () => {
      const loginSpy = spyOn(service, 'login').and.returnValue(of({token: 'asdasda', expiration:new Date()}));
      const routeSpy = spyOn(router, 'navigate').and.callThrough();
      component.form?.get('email')?.setValue('test@test.com');
      component.form?.get('password')?.setValue('1234');
      component.submit(new Event('click'));
      expect(loginSpy).toHaveBeenCalled();
      expect(routeSpy).toHaveBeenCalledWith(['/home']);
    });
  });
});
