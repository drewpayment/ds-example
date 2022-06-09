import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsSetupComponent } from './client-accruals-setup.component';

describe('ClientAccrualsSetupComponent', () => {
  let component: ClientAccrualsSetupComponent;
  let fixture: ComponentFixture<ClientAccrualsSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
