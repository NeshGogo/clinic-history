import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationComponent } from './pagination.component';
import { By } from '@angular/platform-browser';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [PaginationComponent]
    });
    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should go to the next page', () => {
    component.itemsPerPage = 1;
    component.totalItems = 5;
    fixture.detectChanges();
    const btn = fixture.debugElement.query(By.css('#btnNext'));
    btn.triggerEventHandler('click');
    expect(component.currentPage).toBe(2);
  });

  it('should not go to next page because it is last one', () => {
    component.itemsPerPage = 2;
    component.totalItems = 2;
    fixture.detectChanges();
    const btn = fixture.debugElement.query(By.css('#btnNext'));
    btn.triggerEventHandler('click');
    expect(component.currentPage).toBe(1);
  });

  it('should go to the previous page', () => {
    component.itemsPerPage = 2;
    component.totalItems = 5;
    component.currentPage = 2
    fixture.detectChanges();
    const btn = fixture.debugElement.query(By.css('#btnPrev'));
    btn.triggerEventHandler('click');
    expect(component.currentPage).toBe(1);
  });

  it('should not go to previous page because it is first one', () => {
    component.itemsPerPage = 2;
    component.totalItems = 2;
    fixture.detectChanges();
    const btn = fixture.debugElement.query(By.css('#btnNext'));
    btn.triggerEventHandler('click');
    expect(component.currentPage).toBe(1);
  });
});
