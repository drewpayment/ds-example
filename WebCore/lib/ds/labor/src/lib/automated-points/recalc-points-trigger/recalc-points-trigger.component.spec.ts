import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecalcPointsTriggerComponent } from './recalc-points-trigger.component';

describe('RecalcPointsTriggerComponent', () => {
  let component: RecalcPointsTriggerComponent;
  let fixture: ComponentFixture<RecalcPointsTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecalcPointsTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecalcPointsTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
