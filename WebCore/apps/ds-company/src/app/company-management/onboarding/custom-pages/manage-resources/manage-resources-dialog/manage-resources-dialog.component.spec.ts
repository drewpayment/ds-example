import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageResourcesDialogComponent } from './manage-resources-dialog.component';

describe('ManageDocumentDialogComponent', () => {
  let component: ManageResourcesDialogComponent;
  let fixture: ComponentFixture<ManageResourcesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageResourcesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageResourcesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
