import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardSubFormComponent } from './card-sub-form.component';

describe('CardSubFormComponent', () => {
  let component: CardSubFormComponent;
  let fixture: ComponentFixture<CardSubFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardSubFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardSubFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
