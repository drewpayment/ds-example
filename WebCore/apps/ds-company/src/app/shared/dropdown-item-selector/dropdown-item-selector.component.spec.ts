import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownItemSelectorComponent } from './dropdown-item-selector.component';

describe('DropdownItemSelectorComponent', () => {
  let component: DropdownItemSelectorComponent;
  let fixture: ComponentFixture<DropdownItemSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownItemSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownItemSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
