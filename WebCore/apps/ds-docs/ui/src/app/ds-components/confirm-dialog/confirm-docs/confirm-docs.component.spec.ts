import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDocsComponent } from './confirm-docs.component';

describe('ConfirmDocsComponent', () => {
  let component: ConfirmDocsComponent;
  let fixture: ComponentFixture<ConfirmDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
