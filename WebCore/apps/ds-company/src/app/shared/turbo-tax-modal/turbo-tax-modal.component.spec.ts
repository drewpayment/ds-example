import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TurboTaxModalComponent } from './turbo-tax-modal.component';

describe('TurboTaxModalComponent', () => {
  let component: TurboTaxModalComponent;
  let fixture: ComponentFixture<TurboTaxModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TurboTaxModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TurboTaxModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
