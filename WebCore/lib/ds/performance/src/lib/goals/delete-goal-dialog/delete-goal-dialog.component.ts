import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { ICompanyGoal, IGoal } from '../shared/goal.model';
import { GoalService } from '../shared/goal.service';
import * as _ from "lodash";

interface ComponentData {
    user:UserInfo,
    goal:ICompanyGoal,
}

@Component({
    selector: 'ds-delete-goal-dialog',
    templateUrl: './delete-goal-dialog.component.html',
    styleUrls: ['./delete-goal-dialog.component.scss']
})
export class DeleteGoalDialogComponent implements OnInit {
	headerText:string;
	user:UserInfo;
	subGoals:IGoal[];
	goal:ICompanyGoal;

	constructor(public dialogRef:MatDialogRef<DeleteGoalDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data:ComponentData,
        private accountService:AccountService, private goalService:GoalService)
	{

    }

    ngOnInit() {
		this.user = this.data.user;
		this.goal = this.data.goal;
		this.headerText = "Remove '" + this.goal.title + "'";
		if(this.goal.subGoals && this.goal.subGoals.length>0){
			this.headerText += " And Sub Goals?";
			this.subGoals = this.goal.subGoals;
		}else{
			this.headerText += "?";
			this.subGoals = [];
		}
		this.subGoals.forEach(g=> g.isSelectedForDelete = true);
	}

	onNoClick():void {
        this.dialogRef.close();
	}

	deleteGoal():void{
		let notSelected = _.filter(this.subGoals, { 'isSelectedForDelete':false });
		var parentId:number = this.goal.goalId;
		var idsToKeep:number[] = [];
		if(notSelected){
			notSelected.forEach(element => {
				idsToKeep.push(element.goalId);
			});
		}
		var retObject = {
			parentId: parentId,
			idsToKeep:idsToKeep
		}
		this.dialogRef.close(retObject);
	}
}
