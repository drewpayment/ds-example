import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutsDocsComponent } from './layouts-docs.component';

describe('LayoutsDocsComponent', () => {
  let component: LayoutsDocsComponent;
  let fixture: ComponentFixture<LayoutsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
