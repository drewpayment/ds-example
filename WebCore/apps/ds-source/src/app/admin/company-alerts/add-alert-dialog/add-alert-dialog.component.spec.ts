import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAlertDialogComponent } from './add-alert-dialog.component';

describe('AddResourceDialogComponent', () => {
  let component: AddAlertDialogComponent;
  let fixture: ComponentFixture<AddAlertDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAlertDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAlertDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
