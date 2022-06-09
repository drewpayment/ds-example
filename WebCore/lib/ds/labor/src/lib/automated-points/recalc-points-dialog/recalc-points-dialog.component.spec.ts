import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecalcPointsDialogComponent } from './recalc-points-dialog.component';

describe('RecalcPointsDialogComponent', () => {
  let component: RecalcPointsDialogComponent;
  let fixture: ComponentFixture<RecalcPointsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecalcPointsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecalcPointsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
