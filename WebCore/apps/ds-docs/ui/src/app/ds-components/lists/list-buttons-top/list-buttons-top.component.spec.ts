import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListButtonsTopComponent } from './list-buttons-top.component';

describe('ListButtonsTopComponent', () => {
  let component: ListButtonsTopComponent;
  let fixture: ComponentFixture<ListButtonsTopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListButtonsTopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListButtonsTopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
