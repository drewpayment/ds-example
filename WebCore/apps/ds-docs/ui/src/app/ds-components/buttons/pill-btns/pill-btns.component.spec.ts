import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PillBtnsComponent } from './pill-btns.component';

describe('PillBtnsComponent', () => {
  let component: PillBtnsComponent;
  let fixture: ComponentFixture<PillBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PillBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PillBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
