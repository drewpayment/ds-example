import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageWelcomeMessageComponent } from './manage-welcome-message.component';

describe('ManageWelcomeMessageComponent', () => {
  let component: ManageWelcomeMessageComponent;
  let fixture: ComponentFixture<ManageWelcomeMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageWelcomeMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageWelcomeMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
