import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialityComponent } from './speciality.component';

describe('SpecialityComponent', () => {
  let component: SpecialityComponent;
  let fixture: ComponentFixture<SpecialityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [SpecialityComponent]
    });
    fixture = TestBed.createComponent(SpecialityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
