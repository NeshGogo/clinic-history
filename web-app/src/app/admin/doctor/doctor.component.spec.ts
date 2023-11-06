import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorComponent } from './doctor.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DoctorService } from 'src/app/core/services/doctor.service';

describe('DoctorComponent', () => {
  let component: DoctorComponent;
  let fixture: ComponentFixture<DoctorComponent>;
  let service: DoctorService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [DoctorComponent, HttpClientTestingModule]
    });
    fixture = TestBed.createComponent(DoctorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    service = TestBed.inject(DoctorService);
  });

  describe('when initialize', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should fetch doctors', () => {
      const getAllSpyon = spyOn(service, 'getAll').and.callThrough();
      fixture = TestBed.createComponent(DoctorComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
      expect(getAllSpyon).toHaveBeenCalled();
    });
  });

  it('should set displayForm to true', () => {
    expect(component.displayForm).toBe(false);
    component.openForm();
    expect(component.displayForm).toBe(true);
  });

  it('Should fetch data after onSave executed', () => {
    component.displayForm = true;
    const fetchDataSpy = spyOn(component, 'fetchData').and.returnValue();
    fixture.detectChanges();
    component.onSave();
    expect(component.displayForm).toBe(false);
    expect(fetchDataSpy).toHaveBeenCalled();
  });

  it('should disable or enable a doctor', () => {
    const activeOrDisactiveSpy = spyOn(service, 'ActiveOrDisactive').and.callThrough();
    component.onDisabledOrEnable();
    expect(activeOrDisactiveSpy).toHaveBeenCalled();
  });
});
