import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardCalloutComponent } from './card-callout.component';

describe('CardCalloutComponent', () => {
  let component: CardCalloutComponent;
  let fixture: ComponentFixture<CardCalloutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardCalloutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardCalloutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
