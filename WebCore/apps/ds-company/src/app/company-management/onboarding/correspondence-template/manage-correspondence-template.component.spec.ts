import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageCorrespondenceTemplateComponent } from './manage-correspondence-template.component';

describe('ManageCorrespondenceTemplateComponent', () => {
  let component: ManageCorrespondenceTemplateComponent;
  let fixture: ComponentFixture<ManageCorrespondenceTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageCorrespondenceTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageCorrespondenceTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
