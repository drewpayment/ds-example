import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintableFormDocsComponent } from './printable-form-docs.component';

describe('PrintableFormDocsComponent', () => {
  let component: PrintableFormDocsComponent;
  let fixture: ComponentFixture<PrintableFormDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintableFormDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintableFormDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
