import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactNotificationPreferencesFormComponent } from './contact-notification-preferences-form.component';

describe('ContactNotificationPreferencesFormComponent', () => {
  let component: ContactNotificationPreferencesFormComponent;
  let fixture: ComponentFixture<ContactNotificationPreferencesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContactNotificationPreferencesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContactNotificationPreferencesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
