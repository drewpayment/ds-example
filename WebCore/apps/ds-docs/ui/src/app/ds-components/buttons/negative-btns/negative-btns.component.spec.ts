import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NegativeBtnsComponent } from './negative-btns.component';

describe('NegativeBtnsComponent', () => {
  let component: NegativeBtnsComponent;
  let fixture: ComponentFixture<NegativeBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NegativeBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NegativeBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
