import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeBioFormComponent } from './employee-bio-form.component';

describe('EmployeeBioFormComponent', () => {
  let component: EmployeeBioFormComponent;
  let fixture: ComponentFixture<EmployeeBioFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeBioFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeBioFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
