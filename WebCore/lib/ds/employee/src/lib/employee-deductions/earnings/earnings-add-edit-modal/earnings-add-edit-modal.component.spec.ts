import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EarningsAddEditModalComponent } from './earnings-add-edit-modal.component';

describe('EarningsAddEditModalComponent', () => {
  let component: EarningsAddEditModalComponent;
  let fixture: ComponentFixture<EarningsAddEditModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EarningsAddEditModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EarningsAddEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
