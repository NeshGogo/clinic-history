import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisableEnableZoneComponent } from './disable-enable-zone.component';

describe('DisableEnableZoneComponent', () => {
  let component: DisableEnableZoneComponent;
  let fixture: ComponentFixture<DisableEnableZoneComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [DisableEnableZoneComponent]
    });
    fixture = TestBed.createComponent(DisableEnableZoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
