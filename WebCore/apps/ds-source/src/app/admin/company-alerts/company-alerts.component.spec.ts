import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CompanyAlertsComponent } from './company-alerts.component';

describe('CompanyAlertsComponent', () => {
  let component: CompanyAlertsComponent;
  let fixture: ComponentFixture<CompanyAlertsComponent>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyAlertsComponent ]
    })
    .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyAlertsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});