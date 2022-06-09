import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarCalloutComponent } from './avatar-callout.component';

describe('AvatarCalloutComponent', () => {
  let component: AvatarCalloutComponent;
  let fixture: ComponentFixture<AvatarCalloutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarCalloutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarCalloutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
