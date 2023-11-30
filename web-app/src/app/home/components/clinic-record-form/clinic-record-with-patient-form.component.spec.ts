import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicRecordWithPatientFormComponent } from './clinic-record-with-patient-form.component';

describe('ClinicRecordWithPatientFormComponent', () => {
  let component: ClinicRecordWithPatientFormComponent;
  let fixture: ComponentFixture<ClinicRecordWithPatientFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClinicRecordWithPatientFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ClinicRecordWithPatientFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
