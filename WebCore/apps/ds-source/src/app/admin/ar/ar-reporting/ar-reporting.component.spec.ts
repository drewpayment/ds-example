import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArReportingComponent } from './ar-reporting.component';

describe('ArReportingComponent', () => {
  let component: ArReportingComponent;
  let fixture: ComponentFixture<ArReportingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArReportingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArReportingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
