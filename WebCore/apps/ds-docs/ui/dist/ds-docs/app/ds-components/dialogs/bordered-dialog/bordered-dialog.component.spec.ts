import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BorderedDialogComponent } from './bordered-dialog.component';

describe('BorderedDialogComponent', () => {
  let component: BorderedDialogComponent;
  let fixture: ComponentFixture<BorderedDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BorderedDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BorderedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
