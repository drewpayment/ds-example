import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MobileBreadcrumbComponent } from './mobile-breadcrumb.component';

describe('MobileBreadcrumbComponent', () => {
  let component: MobileBreadcrumbComponent;
  let fixture: ComponentFixture<MobileBreadcrumbComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MobileBreadcrumbComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MobileBreadcrumbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
