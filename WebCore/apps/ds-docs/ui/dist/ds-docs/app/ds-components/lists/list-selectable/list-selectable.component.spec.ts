import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSelectableComponent } from './list-selectable.component';

describe('ListSelectableComponent', () => {
  let component: ListSelectableComponent;
  let fixture: ComponentFixture<ListSelectableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListSelectableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListSelectableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
