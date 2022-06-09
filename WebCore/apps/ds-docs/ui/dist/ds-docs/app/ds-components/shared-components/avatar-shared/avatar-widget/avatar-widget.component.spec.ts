import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarWidgetComponent } from './avatar-widget.component';

describe('AvatarWidgetComponent', () => {
  let component: AvatarWidgetComponent;
  let fixture: ComponentFixture<AvatarWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
