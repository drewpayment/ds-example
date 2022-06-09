import * as _ from 'lodash';
import { Component, Inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { TimeAndAttendanceService } from '@ds/payroll/time-and-attendance/time-and-attendance.service';
import { IClockClientHardware } from '@ds/payroll/time-and-attendance/shared/ClockClientHardware.model';
import { UserInfo, UserType } from "@ds/core/shared";
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-time-clock-hardware-edit-dialog',
  templateUrl: './time-clock-hardware-edit-dialog.component.html',
  styleUrls: ['./time-clock-hardware-edit-dialog.component.css']
})
export class TimeClockHardwareEditDialogComponent implements OnInit {

  user:UserInfo;
  clientId: number;
  clockHardware:IClockClientHardware;
  formSubmitted:boolean;
  form:FormGroup;
  readonly momentFormatString = 'MM/DD/YYYY';

  hasFullUpdateAccess: boolean = false;
  hasPartialUpdateAccess: boolean = false;
  hasViewAccess: boolean = false;

  constructor(
    public dialogRef:MatDialogRef<TimeClockHardwareEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:TimeClockHardwareEditDialogComponent,
    private fb:FormBuilder,
    private service:TimeAndAttendanceService,
    private msg:DsMsgService
  ) { 
  }

  ngOnInit():void {
    this.user = this.data.user;
    this.clientId = this.user.lastClientId || this.user.clientId;
    switch(this.user.userTypeId) { 
      case UserType.systemAdmin: { 
        this.hasFullUpdateAccess = true;
        this.hasPartialUpdateAccess = true;
        this.hasViewAccess = true;
        break; 
      } 
      case UserType.companyAdmin: { 
        this.hasFullUpdateAccess = false;
        this.hasPartialUpdateAccess = false;
        this.hasViewAccess = true;
        break; 
      } 
      case UserType.supervisor: { 
        this.hasFullUpdateAccess = false;
        this.hasPartialUpdateAccess = false;
        this.hasViewAccess = true;
        break; 
      } 
      default: { 
         break; 
      } 
   } 

    this.clockHardware = this.data.clockHardware != null
        ? _.cloneDeep(this.data.clockHardware)
        : this.createEmptyClockHardware();

    this.createForm();

    if (!this.hasFullUpdateAccess) {
      this.form.controls.isRental.disable(); 
    }
  }

  onNoClick():void {
    this.dialogRef.close();
  }

  private closeDialogSuccessfully():void {
    this.dialogRef.close(<IClockClientHardware>this.clockHardware);
  }

  saveClockHardware():void {
     this.formSubmitted = true;
     this.form.updateValueAndValidity();
     if(this.form.invalid) return;
      this.msg.sending(true);

      const dto = this.prepareSaveModel();
      this.service.updateClockClientHardware(dto, this.user.lastClientId || this.user.clientId)
          .subscribe(clockHardware => {
              this.clockHardware = clockHardware;

              this.closeDialogSuccessfully();
              this.resetForm(clockHardware);
          });
  }

private createEmptyClockHardware():IClockClientHardware {
    return {
      clockClientHardwareId: null,
      clientId: null,
      description: null,
      model: null,
      email: null,
      ipAddress: null,
      modified: null,
      modifiedBy: null,
      number: null,
      clockClientHardwareFunctionId: null,
      serialNumber: null,
      macAddress: null,
      firmwareVersion: null,
      isRental: null,
      purchaseDate: null,
      warranty: null,
      warrantyEnd: null
    }
}

private createForm():void {
    this.form = this.fb.group({
        description: this.fb.control(this.clockHardware.description || '', [Validators.required]),
        serialNumber: this.fb.control(this.clockHardware.serialNumber || '', [Validators.required]),
        ipAddress: this.fb.control(this.clockHardware.ipAddress || '', [Validators.pattern("(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")]),
        isRental: this.fb.control(this.clockHardware.isRental || false),
        purchaseDate: this.fb.control(this.clockHardware.purchaseDate || '', []),
        warranty: this.fb.control(this.clockHardware.warranty || '', []),
        warrantyEnd: this.fb.control(this.clockHardware.warrantyEnd || '', [])
    });
}

private resetForm(c:IClockClientHardware):void {
    this.formSubmitted = false;
    this.form.reset({
        description: c.description,
        serialNumber: c.serialNumber,
        ipAddress: c.ipAddress,
        isRental: c.isRental,
        purchaseDate: c.purchaseDate,
        warranty: c.warranty,        
        warrantyEnd: c.warrantyEnd
    });
}

private prepareSaveModel():IClockClientHardware {
    return {
      clockClientHardwareId: this.clockHardware != null ? this.clockHardware.clockClientHardwareId : null,
      clientId: this.user.lastClientId || this.user.clientId,
      description: this.form.value.description,
      serialNumber: this.form.value.serialNumber,
      ipAddress: this.form.value.ipAddress,
      isRental: this.form.value.isRental,
      purchaseDate: this.form.value.purchaseDate,
      warranty: this.form.value.warranty,
      warrantyEnd: this.form.value.warrantyEnd,
      model: this.clockHardware.model != null ? this.clockHardware.model : '',
      email: this.clockHardware.email != null ? this.clockHardware.email : '',
      number: this.clockHardware.number != null ? this.clockHardware.number : '',
      firmwareVersion: this.clockHardware.firmwareVersion != null ? this.clockHardware.firmwareVersion : '',
    };
}

}
