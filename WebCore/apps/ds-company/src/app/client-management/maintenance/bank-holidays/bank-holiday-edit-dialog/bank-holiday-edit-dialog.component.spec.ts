import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BankHolidayEditDialogComponent } from './bank-holiday-edit-dialog.component';

describe('BankHolidayEditDialogComponent', () => {
  let component: BankHolidayEditDialogComponent;
  let fixture: ComponentFixture<BankHolidayEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankHolidayEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankHolidayEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
