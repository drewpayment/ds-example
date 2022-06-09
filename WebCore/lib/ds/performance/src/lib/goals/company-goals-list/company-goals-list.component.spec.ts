import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyGoalsListComponent } from './company-goals-list.component';

describe('CompanyGoalsListComponent', () => {
  let component: CompanyGoalsListComponent;
  let fixture: ComponentFixture<CompanyGoalsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyGoalsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyGoalsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
