import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BadgeBtnsComponent } from './badge-btns.component';

describe('BadgeBtnsComponent', () => {
  let component: BadgeBtnsComponent;
  let fixture: ComponentFixture<BadgeBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BadgeBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BadgeBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
