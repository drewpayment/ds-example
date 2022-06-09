import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegendCenterComponent } from './legend-center.component';

describe('LegendCenterComponent', () => {
  let component: LegendCenterComponent;
  let fixture: ComponentFixture<LegendCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegendCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegendCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
