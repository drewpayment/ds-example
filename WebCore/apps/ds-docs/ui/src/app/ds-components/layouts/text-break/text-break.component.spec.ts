import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextBreakComponent } from './text-break.component';

describe('TextBreakComponent', () => {
  let component: TextBreakComponent;
  let fixture: ComponentFixture<TextBreakComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextBreakComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextBreakComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
