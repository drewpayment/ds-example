import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectDepositListComponent } from './direct-deposit-list.component';

describe('DirectDepositListComponent', () => {
  let component: DirectDepositListComponent;
  let fixture: ComponentFixture<DirectDepositListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectDepositListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectDepositListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
