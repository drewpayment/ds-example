import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PillCheckboxComponent } from './pill-checkbox.component';

describe('PillCheckboxComponent', () => {
  let component: PillCheckboxComponent;
  let fixture: ComponentFixture<PillCheckboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PillCheckboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PillCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
