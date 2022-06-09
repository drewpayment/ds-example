import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarTableComponent } from './avatar-table.component';

describe('AvatarTableComponent', () => {
  let component: AvatarTableComponent;
  let fixture: ComponentFixture<AvatarTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
