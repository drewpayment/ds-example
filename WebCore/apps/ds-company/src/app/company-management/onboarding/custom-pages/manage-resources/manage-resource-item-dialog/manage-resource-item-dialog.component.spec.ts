import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageResourceItemDialogComponent } from './manage-resource-item-dialog.component';

describe('ManageResourceDialogComponent', () => {
  let component: ManageResourceItemDialogComponent;
  let fixture: ComponentFixture<ManageResourceItemDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageResourceItemDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageResourceItemDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
