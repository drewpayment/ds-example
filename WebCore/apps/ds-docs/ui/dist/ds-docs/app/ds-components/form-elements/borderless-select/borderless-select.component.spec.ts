import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BorderlessSelectComponent } from './borderless-select.component';

describe('BorderlessSelectComponent', () => {
  let component: BorderlessSelectComponent;
  let fixture: ComponentFixture<BorderlessSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BorderlessSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BorderlessSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
