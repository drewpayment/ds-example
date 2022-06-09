import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartLegendLevelsComponent } from './chart-legend-levels.component';

describe('ChartLegendLevelsComponent', () => {
  let component: ChartLegendLevelsComponent;
  let fixture: ComponentFixture<ChartLegendLevelsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartLegendLevelsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartLegendLevelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
