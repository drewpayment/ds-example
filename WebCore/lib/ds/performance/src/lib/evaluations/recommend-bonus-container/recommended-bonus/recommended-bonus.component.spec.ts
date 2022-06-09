import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendedBonusComponent } from './recommended-bonus.component';

describe('RecommendedBonusComponent', () => {
  let component: RecommendedBonusComponent;
  let fixture: ComponentFixture<RecommendedBonusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecommendedBonusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecommendedBonusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
