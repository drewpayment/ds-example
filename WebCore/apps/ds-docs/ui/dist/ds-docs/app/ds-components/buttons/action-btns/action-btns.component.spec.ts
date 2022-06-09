import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionBtnsComponent } from './action-btns.component';

describe('ActionBtnsComponent', () => {
  let component: ActionBtnsComponent;
  let fixture: ComponentFixture<ActionBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActionBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActionBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
