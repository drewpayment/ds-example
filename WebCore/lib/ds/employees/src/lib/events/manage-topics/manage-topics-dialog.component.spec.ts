import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomizeSenderDialogComponent } from './customize-sender-dialog.component';

describe('CustomizeSenderDialogComponent', () => {
  let component: CustomizeSenderDialogComponent;
  let fixture: ComponentFixture<CustomizeSenderDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomizeSenderDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomizeSenderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
