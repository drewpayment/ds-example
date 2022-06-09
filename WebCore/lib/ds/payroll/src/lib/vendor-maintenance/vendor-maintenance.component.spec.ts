import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VendorMaintenanceComponent } from './vendor-maintenance.component';

describe('VendorMaintenanceComponent', () => {
  let component: VendorMaintenanceComponent;
  let fixture: ComponentFixture<VendorMaintenanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VendorMaintenanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VendorMaintenanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
