import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleDetailCardComponent } from './schedule-detail-card.component';

describe('ScheduleDetailCardComponent', () => {
  let component: ScheduleDetailCardComponent;
  let fixture: ComponentFixture<ScheduleDetailCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleDetailCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleDetailCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
