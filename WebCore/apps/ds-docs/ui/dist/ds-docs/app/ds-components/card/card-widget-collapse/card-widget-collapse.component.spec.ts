import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardWidgetCollapseComponent } from './card-widget-collapse.component';

describe('CardWidgetCollapseComponent', () => {
  let component: CardWidgetCollapseComponent;
  let fixture: ComponentFixture<CardWidgetCollapseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardWidgetCollapseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardWidgetCollapseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
