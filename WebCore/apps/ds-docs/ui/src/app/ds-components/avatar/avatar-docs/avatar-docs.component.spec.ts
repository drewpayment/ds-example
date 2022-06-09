import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvatarDocsComponent } from './avatar-docs.component';

describe('AvatarDocsComponent', () => {
  let component: AvatarDocsComponent;
  let fixture: ComponentFixture<AvatarDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvatarDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvatarDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
