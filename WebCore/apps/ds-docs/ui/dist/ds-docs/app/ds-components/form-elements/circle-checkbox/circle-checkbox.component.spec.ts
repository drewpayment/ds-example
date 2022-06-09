import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CircleCheckboxComponent } from './circle-checkbox.component';

describe('CircleCheckboxComponent', () => {
  let component: CircleCheckboxComponent;
  let fixture: ComponentFixture<CircleCheckboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CircleCheckboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CircleCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
