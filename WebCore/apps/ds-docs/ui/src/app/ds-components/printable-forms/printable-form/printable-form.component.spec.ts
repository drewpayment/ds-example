import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintableFormComponent } from './printable-form.component';

describe('PrintableFormComponent', () => {
  let component: PrintableFormComponent;
  let fixture: ComponentFixture<PrintableFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintableFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintableFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
