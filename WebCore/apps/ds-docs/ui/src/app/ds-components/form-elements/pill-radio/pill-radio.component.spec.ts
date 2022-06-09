import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PillRadioComponent } from './pill-radio.component';

describe('PillRadioComponent', () => {
  let component: PillRadioComponent;
  let fixture: ComponentFixture<PillRadioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PillRadioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PillRadioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
