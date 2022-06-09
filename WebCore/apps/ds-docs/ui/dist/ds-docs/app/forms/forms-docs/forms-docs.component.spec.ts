import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormsDocsComponent } from './forms-docs.component';

describe('FormsDocsComponent', () => {
  let component: FormsDocsComponent;
  let fixture: ComponentFixture<FormsDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormsDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormsDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
