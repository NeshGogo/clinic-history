import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormComponent } from './form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DoctorService } from 'src/app/core/services/doctor.service';
import { of, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { By } from '@angular/platform-browser';
import { Doctor } from 'src/app/core/models/doctor';

describe('FormComponent', () => {
  let component: FormComponent;
  let fixture: ComponentFixture<FormComponent>;
  let service: DoctorService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FormComponent, HttpClientTestingModule],
    });
    fixture = TestBed.createComponent(FormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('when component initialize', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should build the form', () => {
      const keys = ['fullName', 'identification', 'specialityId'];
      const formKeys = Object.keys(component.form.controls);
      expect(formKeys).toEqual(keys);
    });
  });

  describe('When submit method is executed', () => {
    beforeEach(() => {
      service = TestBed.inject(DoctorService);
    });

    it('should not try to add a doctor because btn is disabled', () => {
      const addSpy = spyOn(service, 'add');
      component.submit(new Event('click'));
      expect(addSpy).not.toHaveBeenCalled();
    });

    it('Should show an alert when registration fail', () => {
      const addSpy = spyOn(service, 'add').and.returnValues(
        throwError(() => new HttpErrorResponse({ status: 400 }))
      );
      const showAlertSpy = spyOn<any>(component, 'showAlert');
      component.form?.get('fullName')?.setValue('Test');
      component.form?.get('identification')?.setValue('12345678901');
      component.form
        ?.get('specialityId')
        ?.setValue('123456789012345678901234567890123456');
      component.submit(new Event('click'));
      expect(addSpy).toHaveBeenCalled();
      expect(showAlertSpy).toHaveBeenCalled();
    });

    it('Should disable submit btn and show error if form is invalid', () => {
      const btn: HTMLButtonElement = fixture.debugElement.query(
        By.css('button')
      ).nativeElement;
      expect(
        component.form?.get('fullName')?.hasError('required')
      ).toBeTruthy();
      expect(component.form?.invalid).toBeTruthy();
      expect(btn.disabled).toBeTruthy();
    });

    it('Should register a doctor', () => {
      const entity: Doctor = {
        fullName: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '2112312',
      };
      const addSpy = spyOn(service, 'add').and.returnValue(of(entity));
      component.form?.get('fullName')?.setValue('Test');
      component.form?.get('identification')?.setValue('12345678901');
      component.form
        ?.get('specialityId')
        ?.setValue('123456789012345678901234567890123456');
      component.submit(new Event('click'));
      expect(addSpy).toHaveBeenCalled();
    });
  });

  describe('When is an update', () => {
    const entity: Doctor = {
      fullName: 'Test',
      id: '122313',
      recordCreated: new Date(),
      active: true,
      identification: '12345678901',
      specialityId: '123456789012345678901234567890123456',
    };

    beforeEach(() => {
      fixture = TestBed.createComponent(FormComponent);
      component = fixture.componentInstance;
      component.doctor = entity;
      fixture.detectChanges();
      service = TestBed.inject(DoctorService);
    });

    it('should be form initialize with values', () => {
      const fullName = component.form.value.fullName;
      const identification = component.form.value.identification;
      const specialityId = component.form.value.specialityId;
      expect(fullName).toBeTruthy();
      expect(identification).toBeTruthy();
      expect(specialityId).toBeTruthy();
    });

    it('should update a doctor', () => {
      const entity: Doctor = {
        fullName: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
        identification: '12345678901',
        specialityId: '123456789012345678901234567890123456',
      };
      const updateSpy = spyOn(service, 'update').and.returnValue(of(entity));
      component.form?.get('fullName')?.setValue('Test Test3');
      component.submit(new Event('click'));
      expect(updateSpy).toHaveBeenCalled();
    });
  });
});
