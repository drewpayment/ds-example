import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateValidationComponent } from './template-validation.component';

describe('TemplateValidationComponent', () => {
  let component: TemplateValidationComponent;
  let fixture: ComponentFixture<TemplateValidationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateValidationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
