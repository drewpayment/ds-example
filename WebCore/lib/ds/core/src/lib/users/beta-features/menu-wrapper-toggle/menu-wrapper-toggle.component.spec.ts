import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuWrapperToggleComponent } from './menu-wrapper-toggle.component';

describe('MenuWrapperToggleComponent', () => {
  let component: MenuWrapperToggleComponent;
  let fixture: ComponentFixture<MenuWrapperToggleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuWrapperToggleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuWrapperToggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
