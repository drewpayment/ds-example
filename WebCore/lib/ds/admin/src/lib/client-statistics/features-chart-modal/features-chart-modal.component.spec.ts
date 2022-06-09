import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturesChartModalComponent } from './features-chart-modal.component';

describe('FeaturesChartModalComponent', () => {
  let component: FeaturesChartModalComponent;
  let fixture: ComponentFixture<FeaturesChartModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeaturesChartModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeaturesChartModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
