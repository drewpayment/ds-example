import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencySetupComponent } from './competency-setup.component';

describe('CompetencySetupComponent', () => {
  let component: CompetencySetupComponent;
  let fixture: ComponentFixture<CompetencySetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencySetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencySetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
