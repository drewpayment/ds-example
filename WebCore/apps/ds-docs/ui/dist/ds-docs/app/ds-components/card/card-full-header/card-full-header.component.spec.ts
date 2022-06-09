import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardFullHeaderComponent } from './card-full-header.component';

describe('CardFullHeaderComponent', () => {
  let component: CardFullHeaderComponent;
  let fixture: ComponentFixture<CardFullHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardFullHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardFullHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
