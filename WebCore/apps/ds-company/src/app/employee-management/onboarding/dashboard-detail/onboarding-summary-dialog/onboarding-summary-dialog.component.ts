import * as angular from "angular";
import { IWorkflowItems, IOnboardingSummaryData, IAttachments, IWorkflowResources } from "apps/ds-company/src/app/models/onboarding-data.model";
import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { IOnboardingEmployee } from 'apps/ds-company/src/app/models/onboarding-employee.model';
import { IEmployeeAttachment } from '@ds/core/employees/employee-attachments/employee-attachment.model';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
    selector: 'ds-onboarding-summary-dialog',
    templateUrl: './onboarding-summary-dialog.component.html',
    styleUrls: ['./onboarding-summary-dialog.component.scss']
  })
  export class OnboardingSummaryDialogComponent implements OnInit {

    
    employee: IOnboardingEmployee;
    resources: Array<IEmployeeAttachment>;
    onboardingSummary: IOnboardingSummaryData;
    lastWFIndex: number;    
    

    constructor(
        private ref:MatDialogRef<OnboardingSummaryDialogComponent>,
        private msg: NgxMessageService,
        @Inject(MAT_DIALOG_DATA) public data:any) {
        this.resources  = data.resources;
        this.employee   = data.employee;
    }

    ngOnInit() {
        this.onboardingSummary = {
            companyName: this.employee.clientName,
            submittedDate: this.employee.employeeSignoff,
            employeeName: this.employee.employeeName,
            employeeNumber: this.employee.employeeNumber,
            employeeInitial: this.employee.employeeInitial,
            division: this.employee.divisionName,
            department: this.employee.departmentName,
            supervisor: this.employee.supervisor,
            stateW4: '',
            date: new Date(),
            taxWorkflowItems: [],
            workflowItems: [],
        };

        this.employee.employeeWorkflow.forEach( (workflow, i) => {
            if (workflow.onboardingTask.onboardingWorkflowTaskId == 7 && workflow.isCompleted) { //Workflow title should show the state name in state-w4
                this.onboardingSummary.stateW4 = workflow.formDefinition;
            }
            if (workflow.onboardingTask.onboardingWorkflowTaskId == 3 && workflow.isCompleted) {
                this.onboardingSummary.taxWorkflowItems.push({ id: 1, title: workflow.onboardingTask.workflowTitle, description: workflow.onboardingTask.description, workflowTaskId: 0 });
            }
            else if (workflow.onboardingTask.onboardingWorkflowTaskId == 6 && workflow.isCompleted) {
                this.onboardingSummary.taxWorkflowItems.push({ id: 2, title: workflow.onboardingTask.workflowTitle, description: workflow.onboardingTask.description, workflowTaskId: 0 });
            }
            else if (workflow.onboardingTask.onboardingWorkflowTaskId == 7 && workflow.isCompleted) { //Workflow title should show the state name in state-w4
                this.onboardingSummary.workflowItems.push({ id: 3, title: this.onboardingSummary.stateW4, description: workflow.onboardingTask.description, workflowTaskId: 0 });
            }
            else if (workflow.onboardingTask.mainTaskId == 11 && workflow.onboardingTask.onboardingWorkflowTaskId && workflow.isCompleted && workflow.onboardingTask.signatureDescription) {
                this.onboardingSummary.workflowItems.push({
                    id: i + 3, title: workflow.onboardingTask.workflowTitle,
                    description: workflow.onboardingTask.signatureDescription,
                    workflowTaskId: workflow.onboardingTask.onboardingWorkflowTaskId,
                    resources: workflow.onboardingTask.resources ? _.map(workflow.onboardingTask.resources, (resource) => { return <IWorkflowResources>{ name: resource.resourceName, resourceId: resource.resourceId }; }) : []
                });
            }
        });

        this.onboardingSummary.workflowItems.sort(x=>x.id);
        this.lastWFIndex = this.onboardingSummary.workflowItems.length - 1;
        this.onboardingSummary.workflowItems.forEach( (workflow, i) => {
            if (workflow.id > 3) {
                angular.forEach(this.resources, (resource, i) => {
                    if (resource.onboardingWorkflowTaskId && workflow.workflowTaskId == resource.onboardingWorkflowTaskId) {
                        workflow.resources.push({ name: resource.name, resourceId: resource.resourceId });
                    }
                });
            }
        });
    }

    print = () => {
        var printContent = document.getElementById('printarea').innerHTML;
        var printWindow = window.open('Onboarding Summary', 'Print' + (new Date()).getTime(), 'left=20,top=20,width=0,height=0');
        printWindow.document.write('<!DOCTYPE html><html><head>');

        for (var i = 0; i < document.styleSheets.length; i++) {
            if (document.styleSheets[i].href) {
                printWindow.document.write('<link href= "' + document.styleSheets[i].href + '" rel="stylesheet" type="text/css" >');
            }
        }
        printWindow.document.write('</head><body style="background-color:#fff;">');
        printWindow.document.write(printContent);
        printWindow.document.write('<script>window.print();</script>')
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.focus();

        printWindow.addEventListener('afterprint', (event) => {
            printWindow.close();
        })
    }

    clear() {
        this.ref.close(null);
    }
}
