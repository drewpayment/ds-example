import { Component, OnInit, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { RecommendedBonus } from '../../shared/recommended-bonus';

@Component({
  selector: 'ds-recommended-bonus',
  templateUrl: './recommended-bonus.component.html',
  styleUrls: ['./recommended-bonus.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RecommendedBonusComponent implements OnInit {

@Input() bonus: RecommendedBonus;

  constructor() { }

  ngOnInit() {
  }

  calculateRecommendation(completed: number, total: number): number {
    var result = 0;
    if(completed != null && total != null && total > 0){
      result = ((completed / total) * 100);
    } else {
      result = 100;
    }

    return result;
  }

}
