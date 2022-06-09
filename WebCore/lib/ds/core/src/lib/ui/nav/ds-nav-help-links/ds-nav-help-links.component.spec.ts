import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsNavHelpLinksComponent } from './ds-nav-help-links.component';

describe('DsNavHelpLinksComponent', () => {
  let component: DsNavHelpLinksComponent;
  let fixture: ComponentFixture<DsNavHelpLinksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsNavHelpLinksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsNavHelpLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
