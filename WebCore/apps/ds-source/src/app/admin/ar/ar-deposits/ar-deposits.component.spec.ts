import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArDepositsComponent } from './ar-deposits.component';

describe('ArDepositsComponent', () => {
  let component: ArDepositsComponent;
  let fixture: ComponentFixture<ArDepositsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArDepositsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArDepositsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
