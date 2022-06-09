import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CertifyI9Component } from './certify-I9.component';

describe('CertifyI9Component', () => {
  let component: CertifyI9Component;
  let fixture: ComponentFixture<CertifyI9Component>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CertifyI9Component ]
    })
    .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(CertifyI9Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});