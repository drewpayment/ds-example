import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTimecardComponent } from './employee-timecard.component';

describe('EmployeeTimecardComponent', () => {
  let component: EmployeeTimecardComponent;
  let fixture: ComponentFixture<EmployeeTimecardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTimecardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTimecardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
