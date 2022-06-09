import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckStockDialogComponent } from './check-stock-dialog.component';

describe('CheckStockDialogComponent', () => {
  let component: CheckStockDialogComponent;
  let fixture: ComponentFixture<CheckStockDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckStockDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckStockDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
