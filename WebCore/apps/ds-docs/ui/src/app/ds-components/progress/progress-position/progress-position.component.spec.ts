import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressPositionComponent } from './progress-position.component';

describe('ProgressPositionComponent', () => {
  let component: ProgressPositionComponent;
  let fixture: ComponentFixture<ProgressPositionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressPositionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressPositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
