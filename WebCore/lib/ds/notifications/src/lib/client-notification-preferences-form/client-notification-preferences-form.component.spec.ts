import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientNotificationPreferencesFormComponent } from './client-notification-preferences-form.component';

describe('ClientNotificationPreferencesFormComponent', () => {
  let component: ClientNotificationPreferencesFormComponent;
  let fixture: ComponentFixture<ClientNotificationPreferencesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientNotificationPreferencesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientNotificationPreferencesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
