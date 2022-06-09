import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeferralDialogComponent } from './add-deferral-dialog.component';

describe('AddDeferralDialogComponent', () => {
  let component: AddDeferralDialogComponent;
  let fixture: ComponentFixture<AddDeferralDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddDeferralDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddDeferralDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
