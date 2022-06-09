import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectDepositAddEditModalComponent } from './direct-deposit-add-edit-modal.component';

describe('DirectDepositAddEditModalComponent', () => {
  let component: DirectDepositAddEditModalComponent;
  let fixture: ComponentFixture<DirectDepositAddEditModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectDepositAddEditModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectDepositAddEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
