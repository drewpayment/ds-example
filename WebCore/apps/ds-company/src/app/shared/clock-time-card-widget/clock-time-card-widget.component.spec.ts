import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClockTimeCardWidgetComponent } from './clock-time-card-widget.component';

describe('ClockTimeCardWidgetComponent', () => {
  let component: ClockTimeCardWidgetComponent;
  let fixture: ComponentFixture<ClockTimeCardWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClockTimeCardWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClockTimeCardWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
