import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoFocusDocsComponent } from './auto-focus-docs.component';

describe('AutoFocusDocsComponent', () => {
  let component: AutoFocusDocsComponent;
  let fixture: ComponentFixture<AutoFocusDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutoFocusDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoFocusDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
