import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarColorsComponent } from './avatar-colors.component';

describe('AvatarColorsComponent', () => {
  let component: AvatarColorsComponent;
  let fixture: ComponentFixture<AvatarColorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarColorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarColorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
