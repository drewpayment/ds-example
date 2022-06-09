import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeTrackDocsComponent } from './change-track-docs.component';

describe('ChangeTrackDocsComponent', () => {
  let component: ChangeTrackDocsComponent;
  let fixture: ComponentFixture<ChangeTrackDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeTrackDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeTrackDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
