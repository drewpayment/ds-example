import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BulletListsComponent } from './bullet-lists.component';

describe('BulletListsComponent', () => {
  let component: BulletListsComponent;
  let fixture: ComponentFixture<BulletListsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BulletListsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BulletListsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
