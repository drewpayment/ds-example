import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutomaticBillingDialogComponent } from './automatic-billing-dialog.component';

describe('AutomaticBillingDialogComponent', () => {
  let component: AutomaticBillingDialogComponent;
  let fixture: ComponentFixture<AutomaticBillingDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutomaticBillingDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutomaticBillingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
