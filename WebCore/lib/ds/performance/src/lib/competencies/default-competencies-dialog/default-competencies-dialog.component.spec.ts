import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultCompetenciesDialogComponent } from './default-competencies-.component';

describe('DefaultCompetenciesDialogComponent', () => {
  let component: DefaultCompetenciesDialogComponent;
  let fixture: ComponentFixture<DefaultCompetenciesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultCompetenciesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultCompetenciesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
