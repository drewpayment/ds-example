import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressDocsComponent } from './progress-docs.component';

describe('ProgressDocsComponent', () => {
  let component: ProgressDocsComponent;
  let fixture: ComponentFixture<ProgressDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
