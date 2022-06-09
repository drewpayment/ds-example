import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmpFilterComponent } from './emp-filter.component';

describe('EmpFilterComponent', () => {
  let component: EmpFilterComponent;
  let fixture: ComponentFixture<EmpFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmpFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
