import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutlineIconBtnsComponent } from './outline-icon-btns.component';

describe('OutlineIconBtnsComponent', () => {
  let component: OutlineIconBtnsComponent;
  let fixture: ComponentFixture<OutlineIconBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutlineIconBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutlineIconBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
