import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CertifyI9TriggerComponent } from './certify-I9-trigger.component';

describe('CertifyI9TriggerComponent', () => {
  let component: CertifyI9TriggerComponent;
  let fixture: ComponentFixture<CertifyI9TriggerComponent>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CertifyI9TriggerComponent ]
    })
    .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(CertifyI9TriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});