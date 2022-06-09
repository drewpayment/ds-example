import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaychecksDetailsComponent } from './paychecks-details.component';

describe('PaychecksDetailsComponent', () => {
  let component: PaychecksDetailsComponent;
  let fixture: ComponentFixture<PaychecksDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaychecksDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaychecksDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
