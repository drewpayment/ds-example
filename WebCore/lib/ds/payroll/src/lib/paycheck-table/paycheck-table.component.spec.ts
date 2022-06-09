import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaycheckTableComponent } from './paycheck-table.component';

describe('PaycheckTableComponent', () => {
  let component: PaycheckTableComponent;
  let fixture: ComponentFixture<PaycheckTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaycheckTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaycheckTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
