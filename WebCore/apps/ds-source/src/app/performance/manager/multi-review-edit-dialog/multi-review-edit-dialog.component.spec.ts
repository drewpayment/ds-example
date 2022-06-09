import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiReviewEditDialogComponent } from './multi-review-edit-dialog.component';

describe('MultiReviewEditDialogComponent', () => {
  let component: MultiReviewEditDialogComponent;
  let fixture: ComponentFixture<MultiReviewEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiReviewEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiReviewEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
