import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomChangeTrackComponent } from './custom-change-track.component';

describe('CustomChangeTrackComponent', () => {
  let component: CustomChangeTrackComponent;
  let fixture: ComponentFixture<CustomChangeTrackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomChangeTrackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomChangeTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
