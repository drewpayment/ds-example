import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturesChartComponent } from './features-chart.component';

describe('FeaturesChartComponent', () => {
  let component: FeaturesChartComponent;
  let fixture: ComponentFixture<FeaturesChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeaturesChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturesChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
