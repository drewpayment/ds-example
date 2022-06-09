import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeductionsAddEditModalComponent } from './deductions-add-edit-modal.component';

describe('DeductionsAddEditModalComponent', () => {
  let component: DeductionsAddEditModalComponent;
  let fixture: ComponentFixture<DeductionsAddEditModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeductionsAddEditModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeductionsAddEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
