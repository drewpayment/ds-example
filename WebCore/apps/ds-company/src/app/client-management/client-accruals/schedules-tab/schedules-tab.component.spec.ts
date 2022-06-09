import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulesTabComponent } from './schedules-tab.component';

describe('SchedulesTabComponent', () => {
  let component: SchedulesTabComponent;
  let fixture: ComponentFixture<SchedulesTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulesTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulesTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
