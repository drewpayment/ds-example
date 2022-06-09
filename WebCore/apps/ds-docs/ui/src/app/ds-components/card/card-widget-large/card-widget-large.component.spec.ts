import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardWidgetLargeComponent } from './card-widget-large.component';

describe('CardWidgetLargeComponent', () => {
  let component: CardWidgetLargeComponent;
  let fixture: ComponentFixture<CardWidgetLargeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardWidgetLargeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardWidgetLargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
