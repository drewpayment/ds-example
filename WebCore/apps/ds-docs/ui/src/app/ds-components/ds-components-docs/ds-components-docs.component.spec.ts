import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsComponentsDocsComponent } from './ds-components-docs.component';

describe('DsComponentsDocsComponent', () => {
  let component: DsComponentsDocsComponent;
  let fixture: ComponentFixture<DsComponentsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsComponentsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsComponentsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
