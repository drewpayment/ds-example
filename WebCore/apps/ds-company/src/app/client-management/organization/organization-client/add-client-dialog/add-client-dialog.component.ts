import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
  selector: 'ds-add-client-dialog',
  templateUrl: './add-client-dialog.component.html',
  styleUrls: ['./add-client-dialog.component.scss']
})
export class AddClientDialogComponent implements OnInit {
    searchText: String = '';
    unAssignedClients:Array<any> = [];
    assignedClientsData:Array<any> = [];
    isLoading: boolean = false;
    selectedOrganizationId: number;
    
    constructor(
        public dialogRef:MatDialogRef<AddClientDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any,
        private fb:FormBuilder
    ) {
        
    }

    ngOnInit() {
        this.isLoading = false;
        this.unAssignedClients = this.data.unAssignedClients;
        this.selectedOrganizationId = this.data.selectedOrganizationId;
    }

    unAssignedClientsFiltered = () => {
        return this.unAssignedClients.filter( item => 
            ((item.clientName.toLowerCase().indexOf(this.searchText.toLowerCase()) != -1) || 
             (item.clientCode.toLowerCase().indexOf(this.searchText.toLowerCase()) != -1) || item.isAssigned === true) );
    };

    selectClient(selectedClient) {
        selectedClient.isSelected = !selectedClient.isSelected;
    };

    save():void {
        this.unAssignedClientsFiltered().forEach((unAssignedClient) => {
            if (unAssignedClient.isSelected) {
                this.assignedClientsData.push(unAssignedClient);
            }
        });

        this.dialogRef.close({ selectedClients: this.assignedClientsData });
    }

    onNoClick():void {
        this.dialogRef.close();
    }
}
