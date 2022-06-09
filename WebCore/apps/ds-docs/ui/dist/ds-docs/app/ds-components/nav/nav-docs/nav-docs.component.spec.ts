import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavDocsComponent } from './nav-docs.component';

describe('NavDocsComponent', () => {
  let component: NavDocsComponent;
  let fixture: ComponentFixture<NavDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
