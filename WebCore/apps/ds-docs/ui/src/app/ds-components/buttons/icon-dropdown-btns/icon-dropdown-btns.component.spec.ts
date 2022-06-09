import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IconDropdownBtnsComponent } from './icon-dropdown-btns.component';

describe('IconDropdownBtnsComponent', () => {
  let component: IconDropdownBtnsComponent;
  let fixture: ComponentFixture<IconDropdownBtnsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconDropdownBtnsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconDropdownBtnsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
