import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardObjectComponent } from './card-object.component';

describe('CardObjectComponent', () => {
  let component: CardObjectComponent;
  let fixture: ComponentFixture<CardObjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardObjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardObjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
