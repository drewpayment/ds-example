import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardCalloutIconComponent } from './card-callout-icon.component';

describe('CardCalloutIconComponent', () => {
  let component: CardCalloutIconComponent;
  let fixture: ComponentFixture<CardCalloutIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardCalloutIconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardCalloutIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
