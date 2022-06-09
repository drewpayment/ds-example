import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavMenuComponent } from './ds-nav-menu.component';

describe('DsNavMenuComponent', () => {
  let component: DsNavMenuComponent;
  let fixture: ComponentFixture<DsNavMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
