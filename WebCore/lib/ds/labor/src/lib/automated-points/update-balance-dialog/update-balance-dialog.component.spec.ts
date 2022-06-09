import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBalanceDialogComponent } from './update-balance-dialog.component';

describe('UpdateBalanceDialogComponent', () => {
  let component: UpdateBalanceDialogComponent;
  let fixture: ComponentFixture<UpdateBalanceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateBalanceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateBalanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
