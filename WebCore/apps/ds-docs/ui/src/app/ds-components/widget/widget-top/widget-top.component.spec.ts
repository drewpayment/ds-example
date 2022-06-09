import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WidgetTopComponent } from './widget-top.component';

describe('WidgetTopComponent', () => {
  let component: WidgetTopComponent;
  let fixture: ComponentFixture<WidgetTopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WidgetTopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WidgetTopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
