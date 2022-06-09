import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeofenceManagementComponent } from './geofence-management-map.component';

describe('GeofenceManagementMapComponent', () => {
  let component: GeofenceManagementComponent;
  let fixture: ComponentFixture<GeofenceManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeofenceManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeofenceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
