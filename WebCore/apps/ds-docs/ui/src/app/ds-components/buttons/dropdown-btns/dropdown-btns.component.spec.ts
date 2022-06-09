import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownBtnsComponent } from './dropdown-btns.component';

describe('DropdownBtnsComponent', () => {
  let component: DropdownBtnsComponent;
  let fixture: ComponentFixture<DropdownBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
