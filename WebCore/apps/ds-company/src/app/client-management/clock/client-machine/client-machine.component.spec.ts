import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientMachineComponent } from './client-machine.component';

describe('EditClockHardwareDialogComponent', () => {
  let component: ClientMachineComponent;
  let fixture: ComponentFixture<ClientMachineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientMachineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientMachineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
