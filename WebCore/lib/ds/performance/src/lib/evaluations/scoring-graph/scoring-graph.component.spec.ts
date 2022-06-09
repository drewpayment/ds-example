import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScoringGraphComponent } from './scoring-graph.component';

describe('ScoringGraphComponent', () => {
  let component: ScoringGraphComponent;
  let fixture: ComponentFixture<ScoringGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScoringGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScoringGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
