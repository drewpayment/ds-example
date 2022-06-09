import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateFormChangeTrackComponent } from './template-form-change-track.component';

describe('TemplateFormChangeTrackComponent', () => {
  let component: TemplateFormChangeTrackComponent;
  let fixture: ComponentFixture<TemplateFormChangeTrackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TemplateFormChangeTrackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TemplateFormChangeTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
