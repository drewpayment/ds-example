import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsDialogComponent } from './ds-dialog.component';

describe('DsDialogComponent', () => {
  let component: DsDialogComponent;
  let fixture: ComponentFixture<DsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
