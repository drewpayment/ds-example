import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintableEvaluationComponent } from './printable-evaluation.component';

describe('PrintableEvaluationComponent', () => {
  let component: PrintableEvaluationComponent;
  let fixture: ComponentFixture<PrintableEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintableEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintableEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
