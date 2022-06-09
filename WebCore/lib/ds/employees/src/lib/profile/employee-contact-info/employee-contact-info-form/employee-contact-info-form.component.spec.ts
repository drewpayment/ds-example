import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeContactInfoFormComponent } from './employee-contact-info-form.component';

describe('EmployeeContactInfoFormComponent', () => {
  let component: EmployeeContactInfoFormComponent;
  let fixture: ComponentFixture<EmployeeContactInfoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeContactInfoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeContactInfoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
