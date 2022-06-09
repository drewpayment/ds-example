import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListComplexComponent } from './list-complex.component';

describe('ListComplexComponent', () => {
  let component: ListComplexComponent;
  let fixture: ComponentFixture<ListComplexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListComplexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListComplexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
