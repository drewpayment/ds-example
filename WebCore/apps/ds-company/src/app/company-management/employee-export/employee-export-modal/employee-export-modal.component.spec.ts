import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeExportModalComponent } from './employee-export-modal.component';

describe('EmployeeExportModalComponent', () => {
  let component: EmployeeExportModalComponent;
  let fixture: ComponentFixture<EmployeeExportModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeExportModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeExportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
