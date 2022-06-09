import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormElementsDocsComponent } from './form-elements-docs.component';

describe('FormElementsDocsComponent', () => {
  let component: FormElementsDocsComponent;
  let fixture: ComponentFixture<FormElementsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormElementsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormElementsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
