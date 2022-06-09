import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { IActionNotAllowedRejection } from '@ajs/core/account/account.service';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { DsEmployeeAttachmentModalService } from '@ajs/employee/attachments/addattachment-modal.service';
import { AttachmentsService } from '@ds/performance/attachments/shared/attachments.service';
import { skip } from 'rxjs/operators';
import { EmployeeAttachmentFolderDetail } from '@ds/performance/attachments/shared/models';
import { P } from '@angular/cdk/keycodes';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-employee-performance-nav',
  templateUrl: './employee-performance-nav.component.html',
  styleUrls: ['./employee-performance-nav.component.scss']
})
export class EmployeePerformanceNavComponent implements OnInit {
  canViewGoals = false;
  canViewCompetency = false;
  canViewReviews = false;
  canViewAttachments = false;
  canEditAttachments = false;
  folders : EmployeeAttachmentFolderDetail[] = [];
  user : UserInfo;
  constructor(
	  private accountSvc : AccountService,
	  private attachmentSvc: AttachmentsService,
	  public modalSvc: DsEmployeeAttachmentModalService) {}

  ngOnInit() {
    this.accountSvc.canPerformActions(
		[
			PERFORMANCE_ACTIONS.Performance.ReadCompetencies,
			PERFORMANCE_ACTIONS.Performance.ReadReview,
			PERFORMANCE_ACTIONS.GoalTracking.ReadGoals,
			PERFORMANCE_ACTIONS.Attachment.AddEditAttachments,
			PERFORMANCE_ACTIONS.Attachment.AllEmployeesViewOnly
		]).subscribe(x=> {
			var notAllowed = x as IActionNotAllowedRejection;

			if (notAllowed.actionsNotAllowed && notAllowed.actionsNotAllowed.length ) {

				if (notAllowed.actionsNotAllowed.indexOf("Performance.ReadReview") < 0)
					this.canViewReviews = true;

				if (notAllowed.actionsNotAllowed.indexOf("Performance.ReadCompetencies") < 0)
					this.canViewCompetency = true;

				if (notAllowed.actionsNotAllowed.indexOf("GoalTracking.ReadGoals") < 0)
					this.canViewGoals = true;

				if (notAllowed.actionsNotAllowed.indexOf("Attachment.AllEmployeesViewOnly") < 0)
					this.canViewAttachments = true;

				if (notAllowed.actionsNotAllowed.indexOf("Attachment.AddEditAttachments") < 0)
					this.canEditAttachments = true;

			} else {
				this.canViewReviews = true;
				this.canViewCompetency = true;
				this.canViewGoals = true;
				this.canViewAttachments = true;
				this.canEditAttachments = true;
			}
	});

	this.attachmentSvc.user$.pipe(skip(1)).subscribe((u : UserInfo) => {
		this.user = u;
	});

	this.attachmentSvc.fetchFakeResolver();

  }	// end of on init()

  addAttachment() {
	const currentAttachment = {
		resourceId: 0,
		clientId: this.user.selectedClientId(),
		employeeId: this.user.selectedEmployeeId(),
		name: '',
		folderId: this.attachmentSvc.getFolderId(),
		sourceType: 1,
		source: null,
		isViewableByEmployee: true,
		isNew: true,
		isCompanyAttachment: false
	};

	this.modalSvc.open(currentAttachment, null, this.attachmentSvc.employeeFolders$.value, false).result.then((result) => {

		this.attachmentSvc.setEmployeeFolderList(result.savedFolderList);

	});

  }

}
