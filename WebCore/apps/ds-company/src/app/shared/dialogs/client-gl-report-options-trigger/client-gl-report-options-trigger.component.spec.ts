import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlReportOptionsTriggerComponent } from './client-gl-report-options-trigger.component';

describe('ClientGlReportOptionsTriggerComponent', () => {
  let component: ClientGlReportOptionsTriggerComponent;
  let fixture: ComponentFixture<ClientGlReportOptionsTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlReportOptionsTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlReportOptionsTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
