import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertComponent } from './alert.component';
import { By } from '@angular/platform-browser';

describe('AlertComponent', () => {
  let component: AlertComponent;
  let fixture: ComponentFixture<AlertComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AlertComponent]
    });
    fixture = TestBed.createComponent(AlertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display component with color red, button to close and content with the value test', () => {
    component.color = 'red';
    component.closeable = true;
    component.content = 'test';
    fixture.detectChanges();
    const btn = fixture.debugElement.query(By.css('button'))
    const element: HTMLElement = fixture.nativeElement;
    expect(btn).toBeTruthy();
    expect(element.innerHTML).toContain('test');
    expect(element.innerHTML).toContain('bg-red-50');
  });

  it('should no display the button if property closable is false', () => {
    component.color = 'red';
    component.closeable = false;
    component.content = 'test';
    const btn = fixture.debugElement.query(By.css('button'))
    expect(btn).toBeNull();
  });
});
