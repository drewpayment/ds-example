import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutInlineInputComponent } from './layout-inline-input.component';

describe('LayoutInlineInputComponent', () => {
  let component: LayoutInlineInputComponent;
  let fixture: ComponentFixture<LayoutInlineInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutInlineInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutInlineInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
