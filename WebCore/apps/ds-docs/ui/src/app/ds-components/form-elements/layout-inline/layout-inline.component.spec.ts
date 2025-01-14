import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutInlineComponent } from './layout-inline.component';

describe('LayoutInlineComponent', () => {
  let component: LayoutInlineComponent;
  let fixture: ComponentFixture<LayoutInlineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutInlineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutInlineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
