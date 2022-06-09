import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPostingDialogComponent } from './edit-posting-modal.component';

describe('EditPostingDialogComponent', () => {
  let component: EditPostingDialogComponent;
  let fixture: ComponentFixture<EditPostingDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPostingDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPostingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
