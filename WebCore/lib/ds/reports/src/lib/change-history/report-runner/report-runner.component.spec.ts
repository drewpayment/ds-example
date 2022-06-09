import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportRunnerComponent } from './report-runner.component';

describe('ReportRunnerComponent', () => {
  let component: ReportRunnerComponent;
  let fixture: ComponentFixture<ReportRunnerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportRunnerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportRunnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
