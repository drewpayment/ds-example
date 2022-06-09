import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InputPunchesWidgetComponent } from './input-punches-widget.component';

describe('InputPunchesWidgetComponent', () => {
  let component: InputPunchesWidgetComponent;
  let fixture: ComponentFixture<InputPunchesWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InputPunchesWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InputPunchesWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
