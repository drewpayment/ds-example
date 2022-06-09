import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoCompleteDocsComponent } from './auto-complete-docs.component';

describe('AutoCompleteDocsComponent', () => {
  let component: AutoCompleteDocsComponent;
  let fixture: ComponentFixture<AutoCompleteDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutoCompleteDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoCompleteDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
