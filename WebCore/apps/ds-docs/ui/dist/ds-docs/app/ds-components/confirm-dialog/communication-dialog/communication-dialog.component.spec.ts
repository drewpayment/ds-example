import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationDialogComponent } from './communication-dialog.component';

describe('CommunicationDialogComponent', () => {
  let component: CommunicationDialogComponent;
  let fixture: ComponentFixture<CommunicationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunicationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunicationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
