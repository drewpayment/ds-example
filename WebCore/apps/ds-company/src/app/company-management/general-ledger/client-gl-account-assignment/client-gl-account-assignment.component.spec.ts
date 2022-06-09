import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlAccountAssignmentComponent } from './client-gl-account-assignment.component';

describe('ClientGlAccountAssignmentComponent', () => {
  let component: ClientGlAccountAssignmentComponent;
  let fixture: ComponentFixture<ClientGlAccountAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlAccountAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlAccountAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
