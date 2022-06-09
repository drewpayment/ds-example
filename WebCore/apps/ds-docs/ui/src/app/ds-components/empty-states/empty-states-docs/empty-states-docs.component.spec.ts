import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyStatesDocsComponent } from './empty-states-docs.component';

describe('EmptyStatesDocsComponent', () => {
  let component: EmptyStatesDocsComponent;
  let fixture: ComponentFixture<EmptyStatesDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmptyStatesDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmptyStatesDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
