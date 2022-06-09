import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeBioComponent } from './employee-bio.component';

describe('EmployeeBioComponent', () => {
  let component: EmployeeBioComponent;
  let fixture: ComponentFixture<EmployeeBioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeBioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeBioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
