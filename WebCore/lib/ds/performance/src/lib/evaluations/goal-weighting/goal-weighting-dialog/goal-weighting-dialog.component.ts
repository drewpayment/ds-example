import { Component, OnInit, Inject, ChangeDetectionStrategy} from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder } from '@angular/forms';
import { GoalEvalItem } from '../../shared/goal-eval-item';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';
import { Subject } from 'rxjs';


interface DialogData{
	goalItems: GoalEvalItem[];
	saveApprovalStatusHook: Subject<{item: any, val: number, type: number}>
}

@Component({
	selector: 'ds-goal-weighting-dialog',
    templateUrl: './goal-weighting-dialog.component.html',
    styleUrls: ['./goal-weighting-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class GoalWeightingDialogComponent implements OnInit {

	public goalWeightItems: GoalEvalItem[] = [];
	public totalWeight:number;
	public hasZeroOrNegative:boolean = false;
	private originalWeights: {[id: number]: number};
	constructor(public dialogRef:MatDialogRef<GoalWeightingDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data:DialogData,
		private fb:FormBuilder, private msgSvc:DsMsgService) 
	{ 

	}

  	ngOnInit() {
		this.originalWeights = (this.data.goalItems || []).reduce((prev, next) => {
			prev[next.goal.goalId] = next.goal.weight;
			return prev;
		}, {});

		this.goalWeightItems = this.data.goalItems;
		let nullCounts = 0;
		this.goalWeightItems.forEach(gwi=>{
			if(!gwi.goal.weight){
				nullCounts++;
			}else{
				this.totalWeight +=  (gwi.goal.weight *100);
			}
		});
		if(this.totalWeight>0){
			this.totalWeight = this.totalWeight/100;
		}
		if(nullCounts == this.goalWeightItems.length){
			let totPercent = 0;
			for(var i = 0;i < this.goalWeightItems.length;i++){
				if(i==this.goalWeightItems.length-1){
					this.goalWeightItems[i].goal.weight = 100 - totPercent;
				}else{
					var itemWeight = Number((100 / this.goalWeightItems.length).toFixed(2));
					this.goalWeightItems[i].goal.weight = itemWeight;
					totPercent += itemWeight;
				}
			}
			this.totalWeight = 100;
		}else{
			this.updateTotalWeight();
		}
	}
	  
	onNoClick():void {
        this.dialogRef.close();
	}
	
	saveGoalWeights():void{
		let totPercent = 0;
		this.hasZeroOrNegative = false;
		this.goalWeightItems.forEach(x=>{
			if(x.goal.weight){
				if(x.goal.weight<=0 && x.goal.onReview){
					this.hasZeroOrNegative = true;
				}
				totPercent += ((Number(x.goal.weight))*100);
			}else{
				if(x.goal.onReview)
					this.hasZeroOrNegative = true;
			}
		});
		if(this.hasZeroOrNegative){
			this.msgSvc.setTemporaryMessage('Each goal weight must be greater than 0.', MessageTypes.error, 3000);
			return;
		}
		if(Math.round(totPercent)!=10000)
		{
			this.msgSvc.setTemporaryMessage('Goal weighting total percent must equal 100%.', MessageTypes.error, 3000);
		}
		else{
			this.dialogRef.close(this.goalWeightItems);
		}
	}
	zeroWeight(goalId:number):void{
		var isOnReview = false;
		this.goalWeightItems.forEach(x=>{
			if(x.goal.goalId==goalId)
			{
				x.goal.weight = null;
				isOnReview = x.goal.onReview;
				return;
			}
		}); 
		if(!isOnReview){
			var ct = (<HTMLInputElement>document.getElementById(goalId.toString()));
			ct.classList.remove("is-invalid");
		}
		this.updateTotalWeight();
	}

	updateTotalWeight(ctrlName:string =""){
		this.totalWeight = 0;
		if(ctrlName){
			var ct = (<HTMLInputElement>document.getElementById(ctrlName))
			if(ct.checkValidity()==false)
			{
				if(ct.value == ""){
					ct.classList.add("is-invalid");
				}
				else
				{
					var value = Number(ct.value)
					if(value <=0){
						ct.classList.add("is-invalid");
					}else{
						var val = Number(ct.value);
						var x = ((val * 100) % 1 > 0);
						if(x){
							ct.value = value.toFixed(2);
						}
					}
				}
			}
			else{
				ct.classList.remove("is-invalid")
			}
		}

		var zero = false;
		this.goalWeightItems.forEach(x=>{
			if(x.goal.weight && x.goal.onReview){
				var wat = (Number(x.goal.weight?x.goal.weight:0)).toFixed(2);
				this.totalWeight +=  (Number(wat) *100);
				if(x.goal.weight<0){
					zero = true;
				}
			}else{
				if(x.goal.onReview)
					zero = true;
			}
		});
		this.hasZeroOrNegative = zero;
		if(this.totalWeight>0)
			this.totalWeight = this.totalWeight/100;
	}

	checkWeight(ctrlName:string){
		if(ctrlName){
			var ct = (<HTMLInputElement>document.getElementById(ctrlName))
			if(ct.checkValidity()==false)
			{
				if(ct.value == ""){
					ct.classList.add("is-invalid")
				}
				else
				{
					var value = Number(ct.value)
					if(value <=0){
						ct.classList.add("is-invalid")
					}else{
						var val = Number(ct.value);
						var x = ((val * 100) % 1 > 0);
						if(x){
							ct.value = value.toFixed(2);
						}
					}
				}
			}
		}
	}
}