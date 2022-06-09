import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardCalloutImageComponent } from './card-callout-image.component';

describe('CardCalloutImageComponent', () => {
  let component: CardCalloutImageComponent;
  let fixture: ComponentFixture<CardCalloutImageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardCalloutImageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardCalloutImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
