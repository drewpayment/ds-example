import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PositiveBtnsComponent } from './positive-btns.component';

describe('PositiveBtnsComponent', () => {
  let component: PositiveBtnsComponent;
  let fixture: ComponentFixture<PositiveBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PositiveBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PositiveBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
