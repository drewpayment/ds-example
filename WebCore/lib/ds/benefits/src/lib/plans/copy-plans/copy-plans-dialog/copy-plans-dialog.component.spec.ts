import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyPlansDialogComponent } from './copy-plans-dialog.component';

describe('CopyPlansDialogComponent', () => {
  let component: CopyPlansDialogComponent;
  let fixture: ComponentFixture<CopyPlansDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CopyPlansDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyPlansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
