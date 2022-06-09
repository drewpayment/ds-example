import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorVariationInfoComponent } from './color-variation-info.component';

describe('ColorVariationInfoComponent', () => {
  let component: ColorVariationInfoComponent;
  let fixture: ComponentFixture<ColorVariationInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorVariationInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorVariationInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
