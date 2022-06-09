import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavMenuContentComponent } from './ds-nav-menu-content.component';

describe('DsNavMenuContentComponent', () => {
  let component: DsNavMenuContentComponent;
  let fixture: ComponentFixture<DsNavMenuContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavMenuContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavMenuContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
