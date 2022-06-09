import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppResourceDialogComponent } from './app-resource-dialog.component';

describe('AppResourceDialogComponent', () => {
  let component: AppResourceDialogComponent;
  let fixture: ComponentFixture<AppResourceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppResourceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppResourceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
