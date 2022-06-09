import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyGoalsComponent } from './company-goals.component';

describe('CompanyGoalsComponent', () => {
  let component: CompanyGoalsComponent;
  let fixture: ComponentFixture<CompanyGoalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyGoalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyGoalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
