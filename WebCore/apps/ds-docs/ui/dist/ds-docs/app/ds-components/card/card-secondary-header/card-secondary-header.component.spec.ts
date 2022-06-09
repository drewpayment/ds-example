import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardSecondaryHeaderComponent } from './card-secondary-header.component';

describe('CardSecondaryHeaderComponent', () => {
  let component: CardSecondaryHeaderComponent;
  let fixture: ComponentFixture<CardSecondaryHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardSecondaryHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardSecondaryHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
