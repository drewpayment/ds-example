import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsAtmExportCardComponent } from './client-accruals-atm-export-card.component';

describe('ClientAccrualsAtmExportCardComponent', () => {
  let component: ClientAccrualsAtmExportCardComponent;
  let fixture: ComponentFixture<ClientAccrualsAtmExportCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsAtmExportCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsAtmExportCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
