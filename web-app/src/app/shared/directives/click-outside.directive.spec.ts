import { Component } from '@angular/core';
import { ClickOutsideDirective } from './click-outside.directive';
import { ComponentFixture, TestBed, } from '@angular/core/testing';
import { By } from '@angular/platform-browser';

@Component({
  standalone: true,
  imports: [ClickOutsideDirective],
  template: `
   <div id="inside" appClickOutside (clickOutside)="click()">
        <p>test</p>
      </div>
    <div id="outside" >     
    </div>
  `
})
class HostComponent {
  click(){ }
}

describe('ClickOutsideDirective', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HostComponent, ClickOutsideDirective]
    });

    fixture = TestBed.createComponent(HostComponent);
    fixture.detectChanges();
    component = fixture.componentInstance;
  });

  it('should create an instance', () => {
    const directive = new ClickOutsideDirective(fixture.nativeElement, document);
    expect(directive).toBeTruthy();
  });

  it('should not emit event when the click is inside', () => {
    const clickSpy = spyOn(component, 'click').and.callThrough();
    const inside = fixture.debugElement.query(By.css('#inside')).nativeElement;
    inside.click();
    expect(clickSpy).not.toHaveBeenCalled();
  });

  it('should emit event when the click is outside', () => {
    spyOn(component, 'click').and.callThrough();
    const outside = fixture.debugElement.query(By.css('#outside')).nativeElement;
    outside.click();
    fixture.detectChanges();
    expect(component.click).toHaveBeenCalled();
  });
});
