import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminLayoutComponent } from './admin-layout.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('AdminLayoutComponent', () => {
  let component: AdminLayoutComponent;
  let fixture: ComponentFixture<AdminLayoutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AdminLayoutComponent, HttpClientTestingModule, RouterTestingModule]
    });
    fixture = TestBed.createComponent(AdminLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Should set show to true when close is executed', () => {
    component.show = false;
    fixture.detectChanges();
    component.showMenu();
    expect(component.show).toBeTrue();
  })
});
