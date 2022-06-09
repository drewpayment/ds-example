import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollInformationWidgetComponent } from './payroll-information-widget.component';

describe('PayrollInformationWidgetComponent', () => {
  let component: PayrollInformationWidgetComponent;
  let fixture: ComponentFixture<PayrollInformationWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollInformationWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollInformationWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
