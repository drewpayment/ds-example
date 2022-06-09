import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BankHolidaysComponent } from './bank-holidays.component';

describe('BankHolidaysComponent', () => {
  let component: BankHolidaysComponent;
  let fixture: ComponentFixture<BankHolidaysComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankHolidaysComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankHolidaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});