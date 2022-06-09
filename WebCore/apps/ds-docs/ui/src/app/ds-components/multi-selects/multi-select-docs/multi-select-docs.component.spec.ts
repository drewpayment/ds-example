import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectDocsComponent } from './multi-select-docs.component';

describe('MultiSelectDocsComponent', () => {
  let component: MultiSelectDocsComponent;
  let fixture: ComponentFixture<MultiSelectDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiSelectDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiSelectDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
