import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreadcrumbLayoutComponent } from './breadcrumb-layout.component';

describe('BreadcrumbLayoutComponent', () => {
  let component: BreadcrumbLayoutComponent;
  let fixture: ComponentFixture<BreadcrumbLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreadcrumbLayoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreadcrumbLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
