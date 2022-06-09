import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendBonusContainerComponent } from './recommend-bonus-container.component';

describe('RecommendBonusContainerComponent', () => {
  let component: RecommendBonusContainerComponent;
  let fixture: ComponentFixture<RecommendBonusContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecommendBonusContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecommendBonusContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
