import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutNestingComponent } from './layout-nesting.component';

describe('LayoutNestingComponent', () => {
  let component: LayoutNestingComponent;
  let fixture: ComponentFixture<LayoutNestingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutNestingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutNestingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
