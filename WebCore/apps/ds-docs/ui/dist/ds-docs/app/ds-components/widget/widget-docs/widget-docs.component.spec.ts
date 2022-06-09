import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WidgetDocsComponent } from './widget-docs.component';

describe('WidgetDocsComponent', () => {
  let component: WidgetDocsComponent;
  let fixture: ComponentFixture<WidgetDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WidgetDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WidgetDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
