import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreadcrumbsDocsComponent } from './breadcrumbs-docs.component';

describe('BreadcrumbsDocsComponent', () => {
  let component: BreadcrumbsDocsComponent;
  let fixture: ComponentFixture<BreadcrumbsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreadcrumbsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreadcrumbsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
