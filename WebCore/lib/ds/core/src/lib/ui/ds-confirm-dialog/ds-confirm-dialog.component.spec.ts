import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsConfirmDialogContentComponent } from './ds-confirm-dialog.component';

describe('DsConfirmDialogContentComponent', () => {
  let component: DsConfirmDialogContentComponent;
  let fixture: ComponentFixture<DsConfirmDialogContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsConfirmDialogContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsConfirmDialogContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
