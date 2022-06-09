import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyStatesVerticalComponent } from './empty-states-vertical.component';

describe('EmptyStatesVerticalComponent', () => {
  let component: EmptyStatesVerticalComponent;
  let fixture: ComponentFixture<EmptyStatesVerticalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmptyStatesVerticalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmptyStatesVerticalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
