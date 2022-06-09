import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmergencyContactListComponent } from './emergency-contact-list.component';

describe('EmergencyContactListComponent', () => {
  let component: EmergencyContactListComponent;
  let fixture: ComponentFixture<EmergencyContactListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmergencyContactListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmergencyContactListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
