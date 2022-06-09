import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardHandleComponent } from './card-handle.component';

describe('CardHandleComponent', () => {
  let component: CardHandleComponent;
  let fixture: ComponentFixture<CardHandleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardHandleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardHandleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
