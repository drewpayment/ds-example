import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupervisorOutletComponent } from './supervisor-outlet.component';

describe('SupervisorOutletComponent', () => {
  let component: SupervisorOutletComponent;
  let fixture: ComponentFixture<SupervisorOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupervisorOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupervisorOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
