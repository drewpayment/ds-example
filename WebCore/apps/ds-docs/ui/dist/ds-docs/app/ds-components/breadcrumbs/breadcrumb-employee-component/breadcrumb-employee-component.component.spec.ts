import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreadcrumbEmployeeComponentComponent } from './breadcrumb-employee-component.component';

describe('BreadcrumbEmployeeComponentComponent', () => {
  let component: BreadcrumbEmployeeComponentComponent;
  let fixture: ComponentFixture<BreadcrumbEmployeeComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreadcrumbEmployeeComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreadcrumbEmployeeComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
