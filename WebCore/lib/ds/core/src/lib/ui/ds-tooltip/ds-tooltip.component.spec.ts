import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsTooltipComponent } from './ds-tooltip.component';

describe('DsTooltipComponent', () => {
  let component: DsTooltipComponent;
  let fixture: ComponentFixture<DsTooltipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsTooltipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsTooltipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
