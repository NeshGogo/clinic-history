import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { NavComponent } from './nav.component';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';

describe('NavComponent', () => {
  let component: NavComponent;
  let fixture: ComponentFixture<NavComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [NavComponent, HttpClientTestingModule, RouterTestingModule]
    });
    fixture = TestBed.createComponent(NavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not show the user menu', () => {
    const menu: HTMLDivElement = fixture.debugElement.query(By.css('#user-dropdown')).nativeElement;
    expect( menu.hidden).toBeFalsy();
  });

  it('should open the user menu', () => {
    component.ShowUserMenu(new Event('click'));
    fixture.detectChanges();
    const menu: HTMLDivElement = fixture.debugElement.query(By.css('#user-dropdown')).nativeElement;
    expect( menu.classList.contains('block')).toBeTruthy();
  });

  it('should open the sidebar menu ', () => {
    const showSideMenuSpy = spyOn(component.showSideBarMenu, 'emit').and.callThrough();
    component.showSidebar();
    fixture.detectChanges();
    expect(showSideMenuSpy).toHaveBeenCalled();
  });
});
