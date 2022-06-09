import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileOutletComponent } from './profile-outlet.component';

describe('ProfileOutletComponent', () => {
  let component: ProfileOutletComponent;
  let fixture: ComponentFixture<ProfileOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfileOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
