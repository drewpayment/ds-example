import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeofenceStepperComponent } from './geofence-management-stepper.component';

describe('GeofenceStepperComponent', () => {
  let component: GeofenceStepperComponent;
  let fixture: ComponentFixture<GeofenceStepperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeofenceStepperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeofenceStepperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
