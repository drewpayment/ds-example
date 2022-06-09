import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLargeComponent } from './list-large.component';

describe('ListLargeComponent', () => {
  let component: ListLargeComponent;
  let fixture: ComponentFixture<ListLargeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListLargeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListLargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
