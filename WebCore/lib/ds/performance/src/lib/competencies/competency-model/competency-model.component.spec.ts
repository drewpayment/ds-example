import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyModelComponent } from './competency-model.component';

describe('CompetencyModelComponent', () => {
  let component: CompetencyModelComponent;
  let fixture: ComponentFixture<CompetencyModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
