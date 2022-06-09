import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartColorsComponent } from './chart-colors.component';

describe('ChartColorsComponent', () => {
  let component: ChartColorsComponent;
  let fixture: ComponentFixture<ChartColorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartColorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartColorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
