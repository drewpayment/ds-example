import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RatingsEditDialogComponent } from './ratings-edit-dialog.component';

describe('RatingsEditDialogComponent', () => {
  let component: RatingsEditDialogComponent;
  let fixture: ComponentFixture<RatingsEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RatingsEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RatingsEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
