import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicBreadcrumbComponent } from './basic-breadcrumb.component';

describe('BasicBreadcrumbComponent', () => {
  let component: BasicBreadcrumbComponent;
  let fixture: ComponentFixture<BasicBreadcrumbComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BasicBreadcrumbComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BasicBreadcrumbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
