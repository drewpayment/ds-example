import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyAppComponent } from './company-app.component';

describe('CompanyAppComponent', () => {
  let component: CompanyAppComponent;
  let fixture: ComponentFixture<CompanyAppComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyAppComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
