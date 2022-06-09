import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DateWithPopoverComponent } from './date-with-popover.component';

describe('DateWithPopoverComponent', () => {
  let component: DateWithPopoverComponent;
  let fixture: ComponentFixture<DateWithPopoverComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DateWithPopoverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DateWithPopoverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
