import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormValidationDocsComponent } from './form-validation-docs.component';

describe('FormValidationDocsComponent', () => {
  let component: FormValidationDocsComponent;
  let fixture: ComponentFixture<FormValidationDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormValidationDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormValidationDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
