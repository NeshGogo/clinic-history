import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialityComponent } from './speciality.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SpecialityService } from 'src/app/core/services/speciality.service';

describe('SpecialityComponent', () => {
  let component: SpecialityComponent;
  let fixture: ComponentFixture<SpecialityComponent>;
  let service: SpecialityService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [SpecialityComponent, HttpClientTestingModule],
    });
    fixture = TestBed.createComponent(SpecialityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    service = TestBed.inject(SpecialityService);
  });

  describe('when initialize', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should fetch specialities', () => {
      const getAllSpyon = spyOn(service, 'getAll').and.callThrough();
      fixture = TestBed.createComponent(SpecialityComponent);
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

  it('should disable or enable a speciality', () => {
    const activeOrDisactiveSpy = spyOn(service, 'ActiveOrDisactive').and.callThrough();
    component.onDisabledOrEnable();
    expect(activeOrDisactiveSpy).toHaveBeenCalled();
  });
});
