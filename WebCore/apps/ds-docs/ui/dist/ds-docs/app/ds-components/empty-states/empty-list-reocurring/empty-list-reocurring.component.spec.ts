import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyListReocurringComponent } from './empty-list-reocurring.component';

describe('EmptyListReocurringComponent', () => {
  let component: EmptyListReocurringComponent;
  let fixture: ComponentFixture<EmptyListReocurringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmptyListReocurringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmptyListReocurringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
