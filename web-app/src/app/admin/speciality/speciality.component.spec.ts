import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialityComponent } from './speciality.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SpecialityComponent', () => {
  let component: SpecialityComponent;
  let fixture: ComponentFixture<SpecialityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [SpecialityComponent, HttpClientTestingModule]
    });
    fixture = TestBed.createComponent(SpecialityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
