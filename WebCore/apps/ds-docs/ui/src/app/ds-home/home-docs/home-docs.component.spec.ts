import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeDocsComponent } from './home-docs.component';

describe('HomeDocsComponent', () => {
  let component: HomeDocsComponent;
  let fixture: ComponentFixture<HomeDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HomeDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
