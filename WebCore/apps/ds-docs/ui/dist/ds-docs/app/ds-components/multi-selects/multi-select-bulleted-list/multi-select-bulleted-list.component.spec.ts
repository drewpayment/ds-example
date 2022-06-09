import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectBulletedListComponent } from './multi-select-bulleted-list.component';

describe('MultiSelectBulletedListComponent', () => {
  let component: MultiSelectBulletedListComponent;
  let fixture: ComponentFixture<MultiSelectBulletedListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiSelectBulletedListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiSelectBulletedListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
