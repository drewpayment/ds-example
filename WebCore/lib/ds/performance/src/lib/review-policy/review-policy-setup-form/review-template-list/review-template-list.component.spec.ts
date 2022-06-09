import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewTemplateListComponent } from './review-template-list.component';

describe('ReviewTemplateListComponent', () => {
  let component: ReviewTemplateListComponent;
  let fixture: ComponentFixture<ReviewTemplateListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewTemplateListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewTemplateListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
