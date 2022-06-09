import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactiveFormChangeTrackComponent } from './reactive-form-change-track.component';

describe('ReactiveFormChangeTrackComponent', () => {
  let component: ReactiveFormChangeTrackComponent;
  let fixture: ComponentFixture<ReactiveFormChangeTrackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReactiveFormChangeTrackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReactiveFormChangeTrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
