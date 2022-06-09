import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DayOfMonthSelectorComponent } from './day-of-month-selector.component';

describe('DayOfMonthSelectorComponent', () => {
  let component: DayOfMonthSelectorComponent;
  let fixture: ComponentFixture<DayOfMonthSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DayOfMonthSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DayOfMonthSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
