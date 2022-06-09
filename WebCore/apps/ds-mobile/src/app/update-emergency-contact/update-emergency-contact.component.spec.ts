import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateEmergencyContactComponent } from './update-emergency-contact.component';

describe('UpdateEmergencyContactComponent', () => {
  let component: UpdateEmergencyContactComponent;
  let fixture: ComponentFixture<UpdateEmergencyContactComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateEmergencyContactComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateEmergencyContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
