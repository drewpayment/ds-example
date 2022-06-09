import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StaticBadgeBtnsComponent } from './static-badge-btns.component';

describe('StaticBadgeBtnsComponent', () => {
  let component: StaticBadgeBtnsComponent;
  let fixture: ComponentFixture<StaticBadgeBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StaticBadgeBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StaticBadgeBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
