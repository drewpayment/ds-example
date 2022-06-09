import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsWidgetComponent } from './ds-widget.component';

describe('DsWidgetComponent', () => {
  let component: DsWidgetComponent;
  let fixture: ComponentFixture<DsWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
