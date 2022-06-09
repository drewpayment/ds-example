import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlReportOptionsDialogComponent } from './client-gl-report-options-dialog.component';

describe('ClientGlReportOptionsDialogComponent', () => {
  let component: ClientGlReportOptionsDialogComponent;
  let fixture: ComponentFixture<ClientGlReportOptionsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlReportOptionsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlReportOptionsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
