import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyGoalsHeaderComponent } from './company-goals-header.component';

describe('CompanyGoalsHeaderComponent', () => {
  let component: CompanyGoalsHeaderComponent;
  let fixture: ComponentFixture<CompanyGoalsHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyGoalsHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyGoalsHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
