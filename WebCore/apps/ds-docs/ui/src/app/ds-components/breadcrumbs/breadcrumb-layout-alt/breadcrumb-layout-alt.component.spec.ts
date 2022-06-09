import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreadcrumbLayoutAltComponent } from './breadcrumb-layout-alt.component';

describe('BreadcrumbLayoutAltComponent', () => {
  let component: BreadcrumbLayoutAltComponent;
  let fixture: ComponentFixture<BreadcrumbLayoutAltComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreadcrumbLayoutAltComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreadcrumbLayoutAltComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
