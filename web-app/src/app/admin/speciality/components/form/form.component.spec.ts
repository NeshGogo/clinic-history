import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormComponent } from './form.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SpecialityService } from 'src/app/core/services/speciality.service';
import { of, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { By } from '@angular/platform-browser';
import { Speciality } from 'src/app/core/models/speciality';

describe('FormComponent', () => {
  let component: FormComponent;
  let fixture: ComponentFixture<FormComponent>;
  let service: SpecialityService;

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
      const keys = ['name', 'description'];
      const formKeys = Object.keys(component.form.controls);
      expect(formKeys).toEqual(keys);
    });
  });

  describe('When submit method is executed', () => {
    beforeEach(() => {
      service = TestBed.inject(SpecialityService);
    });

    it('should not try to add a speciality because btn is disabled', () => {
      const addSpy = spyOn(service, 'add');
      component.submit(new Event('click'));
      expect(addSpy).not.toHaveBeenCalled();
    });

    it('Should show an alert when registration fail', () => {
      const addSpy = spyOn(service, 'add').and.returnValues(
        throwError(() => new HttpErrorResponse({ status: 400 }))
      );
      const showAlertSpy = spyOn<any>(component, 'showAlert');
      component.form?.get('name')?.setValue('Test');
      component.submit(new Event('click'));
      expect(addSpy).toHaveBeenCalled();
      expect(showAlertSpy).toHaveBeenCalled();
    });

    it('Should disable submit btn and show error if form is invalid', () => {
      const btn: HTMLButtonElement = fixture.debugElement.query(
        By.css('button')
      ).nativeElement;
      expect(component.form?.get('name')?.hasError('required')).toBeTruthy();
      expect(component.form?.invalid).toBeTruthy();
      expect(btn.disabled).toBeTruthy();
    });

    it('Should navigate to home if authentication is success', () => {
      const speciality: Speciality = {
        name: 'Test Test3',
        id: '12213',
        recordCreated: new Date(),
        active: true,
      };
      const addSpy = spyOn(service, 'add').and.returnValue(
        of(speciality)
      );
      component.form?.get('name')?.setValue('test');
      component.submit(new Event('click'));
      expect(addSpy).toHaveBeenCalled();
    });
  });

  describe('When is an update', () => {
    const entity: Speciality = {
      name: 'Test',
      id: '122313',
      description: 'test',
      recordCreated: new Date(),
      active: true,
    };
    
    it('should form be initialize with values', () => {
      component.speciality = JSON.parse(JSON.stringify(entity));
      fixture.detectChanges();
      const name = component.form.value.name;
      const description = component.form.value.description;
      expect(name).toBeTruthy();
      expect(description).toBeTruthy();
    });
  });
});
