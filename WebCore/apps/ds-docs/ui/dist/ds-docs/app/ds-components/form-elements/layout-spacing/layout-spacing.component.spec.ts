import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutSpacingComponent } from './layout-spacing.component';

describe('LayoutSpacingComponent', () => {
  let component: LayoutSpacingComponent;
  let fixture: ComponentFixture<LayoutSpacingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutSpacingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutSpacingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
